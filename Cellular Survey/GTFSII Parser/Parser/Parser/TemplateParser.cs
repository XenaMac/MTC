using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metra.Module.ServiceAlerts.Models
{
    public class TemplateParser
    {
        private const char NORMAL_DELIMITER_START = '<';
        private const char NORMAL_DELIMITER_END = '>';
        private const char REPEATING_DELIMITER_START = '{';
        private const char REPEATING_DELIMITER_END = '}';
        private const String ONE_OR_MORE = "1";
        private const String PASS_THROUGH_CHAR = "/";
        private const String SINGULAR_PLURAL = "S/P";
        private const String ZERO_OR_MORE = "0";
        private const String NO_DELAY_REASON = "[ none ]";
        private const String PASS_THRU_PLACEHOLDER = "XXXX";
        private const String PASS_THRU_DELIMITER = "xx";
        private const String PASS_THRU_SEPARATOR = "x";
        private const String UNDERSCORE = "_";
        private const String SLASH = "/";
        private const String COLON = ":";

        private Dictionary<string, string> SimpleVariables = new Dictionary<string, string>();

        private void InitializeSimpleVariables()
        {
            SimpleVariables.Add("DIRECTION", "t");
        }

        public TemplateParser()
        {
            InitializeSimpleVariables();
        }

        public class Token
        {
            public Token()
            {
                Init();
            }

            public String VariableName { get; set; }
            public int StartIndex { get; set; }
            public int EndIndex { get; set; }
            public int PrevEndIndex { get; set; }
            public void Init()
            {
                VariableName = "";
                StartIndex = 0;
                EndIndex = 0;
                PrevEndIndex = -1;
            }
        }

        private bool FoundNextToken(String template, Token token)
        {
            try
            {
                int RegDelimIndx = template.IndexOf(NORMAL_DELIMITER_START, token.PrevEndIndex + 1);
                if (RegDelimIndx == -1)
                {
                    // Repeating variables must include normal variables
                    token.StartIndex = template.Length - 1;
                    return false;
                }
                int RepeatDelimIndx = template.IndexOf(REPEATING_DELIMITER_START, token.PrevEndIndex + 1);
                char delimEnd;
                if (RepeatDelimIndx > -1 && RepeatDelimIndx < RegDelimIndx)
                {
                    // Get repeating variable
                    token.StartIndex = RepeatDelimIndx;
                    delimEnd = REPEATING_DELIMITER_END;
                }
                else
                {
                    // Get normal variable
                    token.StartIndex = RegDelimIndx;
                    delimEnd = NORMAL_DELIMITER_END;
                }

                token.EndIndex = template.IndexOf(delimEnd, token.StartIndex);
                if (token.EndIndex == -1)
                {
                    token.StartIndex = token.EndIndex = template.Length - 1;
                    return false;
                }

                token.VariableName = template.Substring(token.StartIndex + 1, token.EndIndex - token.StartIndex - 1);
                return true;
            }
            catch (ArgumentOutOfRangeException outOfRange)
            {
                Console.WriteLine("FoundNextToken Error: {0}", outOfRange.Message);
                return false;
            }
        }

        private String GetPlainText(String template, Token token)
        {
            String parsedMsg = "";
            if (token.StartIndex > 0 && token.PrevEndIndex < token.StartIndex)
            {
                try
                {
                    parsedMsg = template.Substring(token.PrevEndIndex + 1, token.StartIndex - token.PrevEndIndex - 1);
                }
                catch (ArgumentOutOfRangeException outOfRange)
                {
                    Console.WriteLine("CopyPlainText Error: {0}", outOfRange.Message);
                }
                token.PrevEndIndex = token.EndIndex;
            }
            return parsedMsg;
        }

        private String GetDirection(String trainNum)
        {
            // Inbound even number outbound odd
            String direction = "inbound";
            try
            {
                if (Convert.ToInt32(trainNum.Substring(trainNum.Length - 1)) % 2 != 0)
                    direction = "outbound";
            }
            catch (FormatException formatErr)
            {
                Console.WriteLine("GetDirection Error: {0}", formatErr.Message);
            }
            catch (ArgumentOutOfRangeException outOfRange)
            {
                Console.WriteLine("GetDirection Error: {0}", outOfRange.Message);
            }
            return direction;
        }

        private bool HasDelayReason(String delayReason)
        {
            if (delayReason != null && delayReason.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private String GetPassThruText(String oldText)
        {
            return PASS_THRU_DELIMITER + oldText.Replace(UNDERSCORE,
                PASS_THRU_SEPARATOR).Replace(SLASH,
                PASS_THRU_SEPARATOR).Replace(COLON, 
                PASS_THRU_SEPARATOR) + PASS_THRU_DELIMITER;
        }

        private String ParseLineToken(String template, Token token, List<LineAlertInfo> lines, String delayReason)
        {
            String parsedMsg = "";
            // Check for pass through variable
            if (token.VariableName.StartsWith(PASS_THROUGH_CHAR))
            {
                parsedMsg = GetPassThruText(token.VariableName.Substring(1));
            }
            else if (token.VariableName.StartsWith(SINGULAR_PLURAL))
            {
                parsedMsg = GetGrammaticalNumberForm(token.VariableName, lines.Count);
            }
            else if (token.VariableName.StartsWith(ZERO_OR_MORE))
            {
                // Zero or more for single delay reason only
                if (HasDelayReason(delayReason))
                {
                    Token tokenInfo = new Token();
                    String subtemplate = GetQuotedText(token.VariableName);
                    do
                    {
                        if (FoundNextToken(subtemplate, tokenInfo))
                        {
                            parsedMsg = GetPlainText(subtemplate, tokenInfo);
                            parsedMsg += ParseLineToken(subtemplate, tokenInfo, lines, delayReason);
                        }
                        else break;
                    } while (true);
                }
            }
            else if (token.VariableName.StartsWith(ONE_OR_MORE))
            {
                // One or more for list of trains
                Token tokenInfo = new Token();
                List<LineAlertInfo> lineList = new List<LineAlertInfo>();
                String subtemplate = GetQuotedText(token.VariableName);
                int lineCount = lines.Count;
                int count = 1;
                foreach (LineAlertInfo line in lines)
                {
                    lineList.Add(line);
                    do
                    {
                        if (FoundNextToken(subtemplate, tokenInfo))
                        {
                            parsedMsg += GetPlainText(subtemplate, tokenInfo);
                            parsedMsg += ParseLineToken(subtemplate, tokenInfo, lineList, delayReason);
                        }
                        else break;
                    } while (true);

                    if (count <= lineCount - 2)
                        parsedMsg += ", ";
                    else if (count == lineCount - 1)
                    {
                        if (lineCount == 2)
                            parsedMsg += " and ";
                        else
                            parsedMsg += ", and ";
                    }

                    ++count;
                    tokenInfo.Init();
                    lineList.Clear();
                }
            }
            else
            {
                switch (token.VariableName)
                {
                    case "DELAY_REASON":
                        if (HasDelayReason(delayReason))
                        {
                            parsedMsg = delayReason;
                        }
                        else
                        {
                            parsedMsg = GetPassThruText("DELAY_REASON");
                        };
                        break;
                    case "LINE_ABBR":
                        parsedMsg = lines[0].LineAbbr;
                        break;
                    case "LINE":
                        parsedMsg = lines[0].LineName;
                        if (lines[0].SubLineList != null)
                        {
                            parsedMsg += " ";
                            int counter = 1;
                            int nbrSubLines = lines[0].SubLineList.Count;
                            foreach (String subline in lines[0].SubLineList)
                            {
                                parsedMsg += subline;
                                if (counter <= nbrSubLines - 2)
                                    parsedMsg += ", ";
                                else if (counter == nbrSubLines - 1)
                                {
                                    if (nbrSubLines == 2)
                                        parsedMsg += " and ";
                                    else
                                        parsedMsg += ", and ";
                                }
                                ++counter;
                            }
                        }
                        break;
                    default:
                        // Pass through undefined variable
                        parsedMsg = GetPassThruText(token.VariableName);
                        break;
                }
            }
            token.PrevEndIndex = token.EndIndex;
            return parsedMsg;
        }

        public ParsedMessage ParseLineAlert(List<LineAlertInfo> lines, String delayReason, AlertTemplate template)
        {
            if (delayReason.Equals(NO_DELAY_REASON))
            {
                delayReason = null;
            }
            Token tokenInfo = new Token();
            ParsedMessage parsedMsg = new ParsedMessage();
            String header = template.Header.Trim();
            String text = template.Text.Trim();
            // Process Header
            do
            {
                if (FoundNextToken(template.Header, tokenInfo))
                {
                    parsedMsg.AlertSummary += GetPlainText(header, tokenInfo);
                    parsedMsg.AlertSummary += ParseLineToken(header, tokenInfo, lines, delayReason);
                }
                else break;
            } while (true);
            parsedMsg.AlertSummary += header.Substring(tokenInfo.PrevEndIndex + 1, tokenInfo.StartIndex - tokenInfo.PrevEndIndex);
            if (char.IsLower(parsedMsg.AlertSummary[0]))
                parsedMsg.AlertSummary = char.ToUpper(parsedMsg.AlertSummary[0]) + parsedMsg.AlertSummary.Substring(1);

            tokenInfo.Init();
            // Process Text
            do
            {
                if (FoundNextToken(text, tokenInfo))
                {
                    parsedMsg.AlertText += GetPlainText(text, tokenInfo);
                    parsedMsg.AlertText += ParseLineToken(text, tokenInfo, lines, delayReason);
                }
                else break;
            } while (true);
            parsedMsg.AlertText += text.Substring(tokenInfo.PrevEndIndex + 1, tokenInfo.StartIndex - tokenInfo.PrevEndIndex);
            if (char.IsLower(parsedMsg.AlertText[0]))
                parsedMsg.AlertText = char.ToUpper(parsedMsg.AlertText[0]) + parsedMsg.AlertText.Substring(1);

            return parsedMsg;
        }

        private String GetQuotedText(String text)
        {
            String unquotedText = "";
            try
            {
                int valStartIndex = text.IndexOf("'");
                int valEndIndex = text.LastIndexOf("'");
                unquotedText = text.Substring(valStartIndex + 1, valEndIndex - valStartIndex - 1);
            }
            catch (ArgumentOutOfRangeException outOfRange)
            {
                Console.WriteLine("GetQuotedText Error: {0}", outOfRange.Message);
            }
            return unquotedText;
        }

        private String GetGrammaticalNumberForm(String choices, int count)
        {
            String parsedMsg = "";
            try
            {
                String value = GetQuotedText(choices);
                int valMidIndex = value.IndexOf("/");
                if (count > 1)
                    parsedMsg = value.Substring(valMidIndex + 1);
                else
                    parsedMsg = value.Substring(0, valMidIndex);
            }
            catch (ArgumentOutOfRangeException outOfRange)
            {
               Console.WriteLine("GetGrammaticalNumberForm Error: {0}", outOfRange.Message);
            }
            return parsedMsg;
        }

        private String FormatTime(String time24)
        {
            String time12 = "";
            try
            {
                DateTime time = Convert.ToDateTime(time24);
                time12 = time.ToString("h:mm tt");
            }
            catch (FormatException badFormat)
            {
                Console.WriteLine("FormatTime Error: {0}", badFormat.Message);
            }
            return time12;
        }

        private String ParseTrainToken(String template, Token token, List<TrainAlertInfo> trains, String delayReason, 
            int delayTime)
        {
            String parsedMsg = "";
            // Check for pass through variable
            if (token.VariableName.StartsWith(PASS_THROUGH_CHAR))
            {
                 parsedMsg = GetPassThruText(token.VariableName.Substring(1));
            }
            else if (token.VariableName.StartsWith(SINGULAR_PLURAL))
            {
                parsedMsg = GetGrammaticalNumberForm(token.VariableName, trains.Count);
            }
            else if (token.VariableName.StartsWith(ZERO_OR_MORE))
            {
                // Zero or more for single delay reason only
                if (HasDelayReason(delayReason))
                {
                    Token tokenInfo = new Token();
                    String subtemplate = GetQuotedText(token.VariableName);
                    do
                    {
                        if (FoundNextToken(subtemplate, tokenInfo))
                        {
                            parsedMsg = GetPlainText(subtemplate, tokenInfo);
                            parsedMsg += ParseTrainToken(subtemplate, tokenInfo, trains, delayReason, delayTime);
                        }
                        else break;
                    } while (true);
                }
            }
            else if (token.VariableName.StartsWith(ONE_OR_MORE))
            {
                // One or more for list of trains
                Token tokenInfo = new Token();
                List<TrainAlertInfo> taiList = new List<TrainAlertInfo>();
                String subtemplate = GetQuotedText(token.VariableName);
                int trainCount = trains.Count;
                int count = 1;
                foreach (TrainAlertInfo train in trains)
                {
                    taiList.Add(train);
                    do
                    {
                        if (FoundNextToken(subtemplate, tokenInfo))
                        {
                            parsedMsg += GetPlainText(subtemplate, tokenInfo);
                            parsedMsg += ParseTrainToken(subtemplate, tokenInfo, taiList, delayReason, delayTime);
                        }
                        else break;
                    } while (true);

                    if (count <= trainCount - 2)
                        parsedMsg += ", ";
                    else if (count == trainCount - 1)
                    {
                        if (trainCount == 2)
                            parsedMsg += " and ";
                        else
                            parsedMsg += ", and ";
                    }

                    ++count;
                    tokenInfo.Init();
                    taiList.Clear();
                }
            }
            else
            {
                switch (token.VariableName)
                {
                    case "DELAY_REASON":
                        if (HasDelayReason(delayReason))
                        {
                            parsedMsg = delayReason;
                        }
                        else
                        {
                            parsedMsg = GetPassThruText("DELAY_REASON");
                        }
                        break;
                    case "DELAY_TIME":
                        parsedMsg = Convert.ToString(delayTime);
                        break;
                    case "DIRECTION":
                        parsedMsg = GetDirection(trains[0].TrainNum);
                        break;
                    case "SCHED_DESTINATION":
                        parsedMsg = trains[0].SchedDestination;
                        break;
                    case "SCHED_DEST_TIME":
                        parsedMsg = FormatTime(trains[0].SchedDestTime);
                        break;
                    case "SCHED_ORIGIN":
                        parsedMsg = trains[0].SchedOrigin;
                        break;
                    case "SCHED_ORIGIN_TIME":
                        parsedMsg = FormatTime(trains[0].SchedOriginTime);
                        break;
                    case "TRAIN_NUM":
                        parsedMsg = trains[0].TrainNum;
                        break;
                    default:
                        // Pass through undefined variable
                        parsedMsg = GetPassThruText(token.VariableName);
                        break;
                }
            }
            token.PrevEndIndex = token.EndIndex;
            return parsedMsg;
        }

        public ParsedMessage ParseTrainAutoAlert(List<TrainAlertInfo> trains, String delayReason, int delayTime,
            AlertTemplate template)
        {
            if (delayReason.Equals(NO_DELAY_REASON))
            {
                delayReason = null;
            }
            Token tokenInfo = new Token();
            ParsedMessage parsedMsg = new ParsedMessage();
            String header = template.Header.Trim();
            String text = template.Text.Trim();
            // Process Header
            do
            {
                if (FoundNextToken(template.Header, tokenInfo))
                {
                    parsedMsg.AlertSummary += GetPlainText(header, tokenInfo);
                    parsedMsg.AlertSummary += ParseTrainToken(header, tokenInfo, trains, delayReason, delayTime);
                }
                else break;
            } while (true);
            parsedMsg.AlertSummary += header.Substring(tokenInfo.PrevEndIndex + 1, tokenInfo.StartIndex - tokenInfo.PrevEndIndex);
            if (char.IsLower(parsedMsg.AlertSummary[0]))
                parsedMsg.AlertSummary = char.ToUpper(parsedMsg.AlertSummary[0]) + parsedMsg.AlertSummary.Substring(1);

            tokenInfo.Init();
            // Process Text
            do
            {
                if (FoundNextToken(text, tokenInfo))
                {
                    parsedMsg.AlertText += GetPlainText(text, tokenInfo);
                    parsedMsg.AlertText += ParseTrainToken(text, tokenInfo, trains, delayReason, delayTime);
                }
                else break;
            } while (true);
            parsedMsg.AlertText += text.Substring(tokenInfo.PrevEndIndex + 1, tokenInfo.StartIndex - tokenInfo.PrevEndIndex);
            if (char.IsLower(parsedMsg.AlertText[0]))
                parsedMsg.AlertText = char.ToUpper(parsedMsg.AlertText[0]) + parsedMsg.AlertText.Substring(1);

            return parsedMsg;
        }

        public ParsedMessage ParseTrainAlert(List<TrainAlertInfo> trains, String delayReason, AlertTemplate template)
        {
            return ParseTrainAutoAlert(trains, delayReason, 0, template);
        }

        private String ParseStationToken(String template, Token token, List<String> stations)
        {
            String parsedMsg = "";
            // Check for pass through variable
            if (token.VariableName.StartsWith(PASS_THROUGH_CHAR))
            {
                parsedMsg = GetPassThruText(token.VariableName.Substring(1));
            }
            else
            {
                switch (token.VariableName)
                {
                    case "STATION":
                        parsedMsg = stations[0];
                        break;
                    default:
                        // Pass through undefined variable
                        parsedMsg = GetPassThruText(token.VariableName);
                        break;
                }
            }
            token.PrevEndIndex = token.EndIndex;
            return parsedMsg;
        }

        public ParsedMessage ParseStationAlert(List<String> stations, AlertTemplate template)
        {
            Token tokenInfo = new Token();
            ParsedMessage parsedMsg = new ParsedMessage();
            String header = template.Header.Trim();
            String text = template.Text.Trim();
            // Process Header
            do
            {
                if (FoundNextToken(template.Header, tokenInfo))
                {
                    parsedMsg.AlertSummary += GetPlainText(header, tokenInfo);
                    parsedMsg.AlertSummary += ParseStationToken(header, tokenInfo, stations);
                }
                else break;
            } while (true);
            parsedMsg.AlertSummary += header.Substring(tokenInfo.PrevEndIndex + 1, tokenInfo.StartIndex - tokenInfo.PrevEndIndex);
            if (char.IsLower(parsedMsg.AlertSummary[0]))
                parsedMsg.AlertSummary = char.ToUpper(parsedMsg.AlertSummary[0]) + parsedMsg.AlertSummary.Substring(1);

            tokenInfo.Init();
            // Process Text
            do
            {
                if (FoundNextToken(text, tokenInfo))
                {
                    parsedMsg.AlertText += GetPlainText(text, tokenInfo);
                    parsedMsg.AlertText += ParseStationToken(text, tokenInfo, stations);
                }
                else break;
            } while (true);
            parsedMsg.AlertText += text.Substring(tokenInfo.PrevEndIndex + 1, tokenInfo.StartIndex - tokenInfo.PrevEndIndex);
            if (char.IsLower(parsedMsg.AlertText[0]))
                parsedMsg.AlertText = char.ToUpper(parsedMsg.AlertText[0]) + parsedMsg.AlertText.Substring(1);

            return parsedMsg;
        }

        public ParsedMessage ParseAdaAlert(List<String> stations, AlertTemplate template)
        {
            return ParseStationAlert(stations, template);
        }

        public ParsedMessage ParseElevatorAlert(List<String> stations, AlertTemplate template)
        {
            return ParseStationAlert(stations, template);
        }

        public ParsedMessage ParseSystemAlert(AlertTemplate template)
        {
            List<String> stations = new List<String>();
            stations.Add("");
            return ParseStationAlert(stations, template);
        }

    }
}
