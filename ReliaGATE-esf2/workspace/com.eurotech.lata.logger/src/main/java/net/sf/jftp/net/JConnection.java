/*
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.

 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 */
package net.sf.jftp.net;

import java.io.BufferedOutputStream;
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintStream;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.NetworkInterface;
import java.net.Socket;
import java.net.SocketAddress;
import java.util.Enumeration;
import java.util.NoSuchElementException;

import net.sf.jftp.config.Settings;
import net.sf.jftp.system.logging.Log;


/**
 * Basic class for getting an TCP/IP-Connection
 * timeout sets (as the name says) the maximum time the Thread
 * waits for the target host...
 */
public class JConnection implements Runnable
{
    private int timeout = Settings.connectionTimeout;
    private String host;
    private String netIntrfc = "";
    private int port;
    private PrintStream out;
    private BufferedReader in;
    private Socket s;
    private boolean isOk = false;
    private boolean established = false;
    private boolean reciever = false;
    private Thread runner;
    private int localPort = -1;
    private int time = 0;

    /*
    private boolean useSocks4 = false;
    private String socks4Host = "192.168.0.1";
    private int socks4Port = 1080;
    */
    public JConnection(String host, int port)
    {
        this.host = host;
        this.port = port;

        runner = new Thread(this);
        runner.start();
    }

    public JConnection(String host, String netIntrfc, int port)
    {
        this.host = host;
        this.netIntrfc = netIntrfc;
        this.port = port;

        runner = new Thread(this);
        runner.start();
    }
    
    public JConnection(String host, int port, int time)
    {
        this.host = host;
        this.port = port;
        this.timeout = time;
        this.time = time;

        runner = new Thread(this);
        runner.start();
    }

    /*
    private int swabInt(int v)
    {
            return  (v >>> 24) | (v << 24) |
      ((v << 8) & 0x00FF0000) | ((v >> 8) & 0x0000FF00);
    }
    */
    public void run()
    {
        try
        {
            /*
                if(useSocks4)
                {
                        s = new Socket(socks4Host, socks4Port);

                    Log.debug("\nsending socks4-request...");

                    OutputStream o = s.getOutputStream();

                    byte buf[] = new byte[9];

                    buf[0] = (byte) 4;
                    buf[1] = (byte) 0x1;
                    buf[2] = (byte) 1;
                    buf[3] = (byte) 1;
                    buf[4] = (byte) 1;
                    buf[5] = (byte) 1;
                    buf[6] = (byte) 1;
                    buf[7] = (byte) 1;
                    buf[8] = (byte) 0;

                    o.write(buf);
                    o.flush();

                        Log.debug("reading response...");
                    byte ret[] = new byte[8];
                    int bytes = s.getInputStream().read(ret);

                    if(ret[1] == 90)
                    {
                            Log.debug("connection accepted");

                                localPort = s.getLocalPort();
                                out = new PrintStream(new BufferedOutputStream(s.getOutputStream(), Settings.bufferSize));
                                in = new BufferedReader(new InputStreamReader(s.getInputStream()), Settings.bufferSize);
                            isOk = true;
                    }
                    else
                    {
                            Log.debug("socks-connection refused (returncode " + ret[1] + ")");
                            isOk = false;
                    }
                }
                else
                {
                */
        	
        	/*
        	 * List network interfaces
        	 */
/*          logWrite("netIntrfc: " + netIntrfc);
    		Enumeration networkInterfaces = NetworkInterface.getNetworkInterfaces();
    		while(networkInterfaces.hasMoreElements()) {
    			NetworkInterface networkInterface = (NetworkInterface) networkInterfaces.nextElement();
    			logWrite(networkInterface.getName());
    			logWrite(networkInterface.getDisplayName());
    		}*/
        	
        	/*
        	 * Was trying to pass network interface name in netIntrfc
        	 * but networkInterface.getInetAddresses returns IPv6 addresses on DynaCOR
        	 * now passing network interface address
        	 */

/*        NetworkInterface networkInterface = NetworkInterface.getByName(netIntrfc);
        	Enumeration nifAddresses = networkInterface.getInetAddresses();
        	InetAddress netIntrfcAddr = (InetAddress) nifAddresses.nextElement();
        	logWrite(" manual netIntrfcAddr getLocalAddress " + netIntrfcAddr.getHostAddress());*/
   
          s = new Socket();
        	s.bind(new InetSocketAddress(netIntrfc, 0));
        	s.connect(new InetSocketAddress(host, port));
        	localPort = s.getLocalPort();
        	 
        	/*logWrite(" auto getLocalAddress " + s.getLocalAddress().getHostAddress());
        	logWrite(" auto localPort " + localPort);*/


            //if(time > 0) s.setSoTimeout(time);
            out = new PrintStream(new BufferedOutputStream(s.getOutputStream(),
                                                           Settings.bufferSize));
            in = new BufferedReader(new InputStreamReader(s.getInputStream()),
                                    Settings.bufferSize);
            isOk = true;

            // }
        }
        catch(Throwable ex)
        {
          logWrite(" Exception " + ex.toString());
            ex.printStackTrace();
            Log.out("WARNING: connection closed due to exception (" + host +
                    ":" + port + ")");
            isOk = false;

            try
            {
                if((s != null) && !s.isClosed())
                {
                    s.close();
                }

                if(out != null)
                {
                    out.close();
                }

                if(in != null)
                {
                    in.close();
                }
            }
            catch(Exception ex2)
            {
                ex2.printStackTrace();
                Log.out("WARNING: got more errors trying to close socket and streams");
            }
        }

        established = true;
    }

    public boolean isThere()
    {
        int cnt = 0;

        while(!established && (cnt < timeout))
        {
            pause(10);
            cnt = cnt + 10;

            //System.out.println(cnt + "/" + timeout);
        }

        return isOk;
    }

    public void send(String data)
    {
        try
        {
            //System.out.println(":"+data+":");
            out.print(data);
            out.print("\r\n");
            out.flush();

            if(data.startsWith("PASS"))
            {
                Log.debug("> PASS ****");
            }
            else
            {
                Log.debug("> " + data);
            }
        }
        catch(Exception ex)
        {
            ex.printStackTrace();
        }
    }

    public PrintStream getInetOutputStream()
    {
        return out;
    }

    public BufferedReader getReader()
    {
        return in;
    }

    public int getLocalPort()
    {
        return localPort;
    }

    public InetAddress getLocalAddress() throws IOException
    {
        return s.getLocalAddress();
    }

    private void pause(int time)
    {
        try
        {
            Thread.sleep(time);
        }
        catch(Exception ex)
        {
        }
    }

	public BufferedReader getIn() {
		return in;
	}

	public void setIn(BufferedReader in) {
		this.in = in;
	}

	public PrintStream getOut() {
		return out;
	}

	public void setOut(PrintStream out) {
		this.out = out;
	}
	
	private void logWrite(String msg) {

	  try {
	    BufferedWriter writer =
	        new BufferedWriter(new FileWriter("/lata/logs/JConnection.log", true));
	    writer.write(msg + "\n");
	    writer.close();
	  }
	  catch (IOException ioe) {
	  }
	}
  
/*	private byte[] getAddrByteArr(int a1, int a2, int a3, int a4) {
	  byte[] b1 = new byte[4];
	  b1[ 0] = new Integer(a1).byteValue();
	  b1[ 1] = new Integer(a2).byteValue();
	  b1[ 2] = new Integer(a3).byteValue();
	  b1[ 3] = new Integer(a4).byteValue();
	  return b1;
	}*/
}
