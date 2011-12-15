package com.example;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.List;

import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;

import android.app.ProgressDialog;
import android.util.Log;

/*
 * Aparch client
 */
public class HttpConnection
{
//	private final static String URL="http://10.0.2.2/sms/incoming.php";
	
	private final static String URL="http://taxi.urvoting.com/sms/incoming.php";
	
	public HttpConnection()
	{}
	
	public void httpConnect(String source, String destination, String mobileNum)
	{
		
		String url = URL;
		
		HttpClient client = null;
		HttpPost request = null;
		HttpResponse response=null;
		String msg="";
		//send the requst via POST
		List<NameValuePair> formparams = new ArrayList<NameValuePair>();
		formparams.add(new BasicNameValuePair("msg", source+" To "+destination));
		formparams.add(new BasicNameValuePair("from", mobileNum));
		try
		{
			UrlEncodedFormEntity entity = new UrlEncodedFormEntity(formparams, "UTF-8");
		    request = new HttpPost(url);
			request.setEntity(entity);
			client = new DefaultHttpClient();
			response=client.execute(request);
			
			msg=getResponse(response);
			Log.i("debug",msg);
			
		}			
		catch(Exception ex)
		{
			Log.i("debug",ex.getMessage());
		}
		
	}
	
	public void sendCancel(String mobileNum)
	{
		String url=URL;
		HttpResponse response=null;
		String msg="";
		
		//send the cancel via POST
		HttpClient client = null;
		HttpPost request = null;
		List<NameValuePair> formparams = new ArrayList<NameValuePair>();
		
		formparams.add(new BasicNameValuePair("msg", "CALL"));
		formparams.add(new BasicNameValuePair("from", mobileNum));
		try
		{
			UrlEncodedFormEntity entity = new UrlEncodedFormEntity(formparams, "UTF-8");
		    request = new HttpPost(url);
			request.setEntity(entity);
			client = new DefaultHttpClient();
		   	response=client.execute(request);
		   	msg=getResponse(response);
		}			
		catch(Exception ex)
		{
			Log.i("debug",ex.getMessage());
		}
		
	}
	
	
	public String getResponse(HttpResponse response)
	{
		String text = "";
		InputStream in=null;
		try
		{
		    in =response.getEntity().getContent(); 
			BufferedReader reader = new BufferedReader(new InputStreamReader(in));
			text=reader.readLine();
		}
		catch(Exception ex)
		{
			Log.i("debug",ex.toString());
		}
		finally
		{
		
			try
			{

				in.close();
			}
			catch(Exception ex) {}
			
		}
		return text;
		
		
	}
}
