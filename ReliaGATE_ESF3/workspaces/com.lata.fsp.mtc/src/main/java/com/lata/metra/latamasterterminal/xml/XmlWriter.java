package com.lata.metra.latamasterterminal.xml;

import java.io.IOException;
import java.io.Writer;

import java.util.Stack;

public class XmlWriter {

    private Writer writer;      // underlying writer
    private Stack stack;        // of xml entity names
    private StringBuffer attrs; // current attribute string
    private boolean empty;      // is the current node empty
    private boolean closed;     // is the current node closed...

    /**
     * Create an XmlWriter on top of an existing java.io.Writer.
     */
    public XmlWriter(Writer writer) {
        this.writer = writer;
        this.closed = true;
        this.stack = new Stack();
    }

    /**
     * Begin to output an entity. 
     *
     * @param String name of entity.
     */
    public XmlWriter writeEntity(String name) throws XmlWriterException {
        try {
            closeOpeningTag();
            this.closed = false;
            this.writer.write("<");
            this.writer.write(name);
            stack.add(name);
            this.empty = true;
            return this;
        } catch (IOException ioe) {
            throw new XmlWriterException(ioe);
        }
    }

    // close off the opening tag
    private void closeOpeningTag() throws IOException {
        if (!this.closed) {
            writeAttributes();
            this.closed = true;
            this.writer.write(">");
        }
    }

    // write out all current attributes
    private void writeAttributes() throws IOException {
        if (this.attrs != null) {
            this.writer.write(this.attrs.toString());
            this.attrs.setLength(0);
            this.empty = false;
        }
    }

    /**
     * Write an attribute out for the current entity. 
     * Any xml characters in the value are escaped.
     * Currently it does not actually throw the exception, but 
     * the api is set that way for future changes.
     *
     * @param String name of attribute.
     * @param String value of attribute.
     */
    public XmlWriter writeAttribute(String attr, String value) throws XmlWriterException {

        if (this.attrs == null) {
            this.attrs = new StringBuffer();
        }
        
        this.attrs.append(" ");
        this.attrs.append(attr);
        this.attrs.append("=\"");
        this.attrs.append(escapeXml(value));
        this.attrs.append("\"");
        
        return this;
    }

    /**
     * End the current entity. This will throw an exception 
     * if it is called when there is not a currently open 
     * entity.
     */
    public XmlWriter endEntity() throws XmlWriterException {
        try {
            if(this.stack.empty()) {
                throw new XmlWriterException("Called endEntity too many times. ");
            }
            String name = (String)this.stack.pop();
            if (name != null) {
                if (this.empty) {
                    writeAttributes();
                    this.writer.write("/>");
                } else {
                    this.writer.write("</");
                    this.writer.write(name);
                    this.writer.write(">");
                }
                this.empty = false;
            }
            return this;
        } catch (IOException ioe) {
            throw new XmlWriterException(ioe);
        }
    }

    /**
     * Close this writer. It does not close the underlying 
     * writer, but does throw an exception if there are 
     * as yet unclosed tags.
     */
    public void close() throws XmlWriterException {
        if(!this.stack.empty()) {
            throw new XmlWriterException("Tags are not all closed. "+
                "Possibly, "+this.stack.pop()+" is unclosed. ");
        }
    }

    /**
     * Output body text. Any xml characters are escaped. 
     */
    public XmlWriter writeText(String text) throws XmlWriterException {
        try {
            closeOpeningTag();
            this.empty = false;
            this.writer.write(escapeXml(text));
            return this;
        } catch (IOException ioe) {
            throw new XmlWriterException(ioe);
        }
    }

    static public String escapeXml(String str) {
        str = replaceString(str,"&","&amp;");
        str = replaceString(str,"<","&lt;");
        str = replaceString(str,">","&gt;");
        str = replaceString(str,"\"","&quot;");
        str = replaceString(str,"'","&apos;");
        return str;
    }  

    // from StringW
    static public String replaceString(String text, String repl, String with) {
        return replaceString(text, repl, with, -1);
    }  

    /**
     * Replace a string with another string inside a larger string, for
     * the first n values of the search string.
     *
     * @param text String to do search and replace in
     * @param repl String to search for
     * @param with String to replace with
     * @param n    int    values to replace
     *
     * @return String with n values replacEd
     */
    static public String replaceString(String text, String repl, String with, int max) {
        if(text == null) {
            return null;
        }
 
        StringBuffer buffer = new StringBuffer(text.length());
        int start = 0;
        int end = 0;
        while( (end = text.indexOf(repl, start)) != -1 ) {
            buffer.append(text.substring(start, end)).append(with);
            start = end + repl.length();
 
            if(--max == 0) {
                break;
            }
        }
        buffer.append(text.substring(start));
 
        return buffer.toString();
    }              

    static public void test1() throws XmlWriterException {
        Writer writer = new java.io.StringWriter();
        XmlWriter xmlwriter = new XmlWriter(writer);
        xmlwriter.writeEntity("person").writeAttribute("name", "fred").writeAttribute("age", "12").writeEntity("phone").writeText("4254343").endEntity().writeEntity("bob").endEntity().endEntity();
        xmlwriter.close();
        System.err.println(writer.toString());
    }
    
    static public void test2() throws XmlWriterException {
        Writer writer = new java.io.StringWriter();
        XmlWriter xmlwriter = new XmlWriter(writer);
        xmlwriter.writeEntity("person");
        xmlwriter.writeAttribute("name", "fred");
        xmlwriter.writeAttribute("age", "12");
        xmlwriter.writeEntity("phone");
        xmlwriter.writeText("4254343");
        xmlwriter.endEntity();
        xmlwriter.writeEntity("bob");
        xmlwriter.endEntity();
        xmlwriter.endEntity();
        xmlwriter.close();
        System.err.println(writer.toString());
    }
}
