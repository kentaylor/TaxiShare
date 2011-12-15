package com.example;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import java.util.StringTokenizer;

import com.google.android.maps.GeoPoint;
import com.google.android.maps.MapActivity;
import com.google.android.maps.MapController;
import com.google.android.maps.MapView;
import com.google.android.maps.Overlay;
import com.google.android.maps.OverlayItem;

import android.app.AlertDialog;
import android.app.Dialog;
import android.app.ProgressDialog;
import android.app.SearchManager;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.drawable.Drawable;
import android.location.Address;
import android.location.Geocoder;
import android.net.ConnectivityManager;
import android.net.Uri;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.widget.Button;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.RelativeLayout;
import android.widget.Toast;

/*
 *  This is main control activity 
 *  extend MapActivity to show the google map view
 */
public class TaxiShareActivity extends MapActivity implements View.OnClickListener
{
	public final static String tag="debug";
	private final static int SEND_REQUEST=1;
	private final static int ENTER_NUM=3;
	private final static int CHECK_NUM=4;
	private final static int INFO_DIALOG=5;
	private final static int NET_CONNECT=6;
	private static final String PREFS_NAME = "mobileNumPrefs";
	private MapController mapController;
	private MapView mapView;
	private MapItemizedOverlay fromOverlay;
	private MapItemizedOverlay toOverlay;
	private List<Overlay> mapOverlays;
	private Drawable fromDrawable;
	private Drawable toDrawable;
	private RadioButton from_radio;
	private RadioButton to_radio;
	private RadioButton map_radio;
	private RadioGroup marker_radiogroup;
	private Button reset_button;
	private RelativeLayout mapLayout; 
	private TransparentPanel markerLayout;
	private String mobileNumber=null;
	private ProgressDialog progressDialog;
	private String mResult;
	private String fromLocation;
	private String toLocation;

	/*
	 * Initialise all the views (mapview, maplayout) 
	 * 
	 * @see com.google.android.maps.MapActivity#onCreate(android.os.Bundle)
	 */
	
	public void onCreate(Bundle savedInstanceState)
	{
		
		super.onCreate(savedInstanceState);
		setContentView(R.layout.main);
	
		//check the internet connection before start the application
		ConnectivityManager connec = (ConnectivityManager)getSystemService(this.CONNECTIVITY_SERVICE); 
		if(!connec.getNetworkInfo(0).isConnectedOrConnecting()&&!connec.getNetworkInfo(1).isConnectedOrConnecting())
		{
			showDialog(NET_CONNECT);
		}else
		{
		
			SharedPreferences settings = getSharedPreferences(PREFS_NAME, 0);
			mobileNumber = settings.getString("mobileNumber", "null");
		
		
			if(mobileNumber.equals("null"))
			{
				showDialog(ENTER_NUM);
			}else
			{
				showDialog(CHECK_NUM);
			}
		}
		
		mapView = (MapView) findViewById(R.id.mapview);
	    mapView.setBuiltInZoomControls(true);
	   
	    mapController = mapView.getController();
	    mapOverlays=mapView.getOverlays();
	    
	    fromDrawable = this.getResources().getDrawable(R.drawable.frommarker);
	    toDrawable = this.getResources().getDrawable(R.drawable.tomarker);
	    
	    fromOverlay = new MapItemizedOverlay(fromDrawable);
	    toOverlay = new MapItemizedOverlay(toDrawable);
	    
	    //set the google map start at Australia 
	    GeoPoint startpoint = new GeoPoint(-25274398,133775136);
	    mapController.animateTo(startpoint);
	    mapController.setZoom(4);
	    
	    mapLayout = (RelativeLayout) findViewById(R.id.map_layout);
	    //load the marker panel layout from XML
	    LayoutInflater inflater = getLayoutInflater();
	    View view = inflater.inflate(R.layout.markerlayout, null);
	    markerLayout = (TransparentPanel) view.findViewById(R.id.marker_layout);
	    from_radio = (RadioButton) view.findViewById(R.id.from_radio);
		to_radio =(RadioButton)view.findViewById(R.id.to_radio);
	    map_radio=(RadioButton)view.findViewById(R.id.map_radio);	
		marker_radiogroup=(RadioGroup)view.findViewById(R.id.marker_radiogroup);
		reset_button=(Button)view.findViewById(R.id.reset_button);
		
		from_radio.setOnClickListener(this);
		to_radio.setOnClickListener(this);
		reset_button.setOnClickListener(this);
	    map_radio.setOnClickListener(this);
	    
	    mapView.setSatellite(false);
		mapView.setStreetView(false);
		mapView.setTraffic(false);
	   
	    
	}
		

	/*
	 * Store the mobile number before quit the application 
	 * @see android.app.Activity#onStop()
	 */
	
	protected void onStop()
	{
		super.onStop();
		 
		//store mobile number in preference 
		SharedPreferences settings = getSharedPreferences(PREFS_NAME, 0);
	    SharedPreferences.Editor editor = settings.edit();
	    editor.putString("mobileNumber", mobileNumber);

	    // to commit your edits!!!
	    editor.commit();
		
	}
	
	/*
	 * create the control menu, load the menu from options_menu XML
	 * @see android.app.Activity#onCreateOptionsMenu(android.view.Menu)
	 */
	public boolean onCreateOptionsMenu(Menu menu)
	{
		MenuInflater inflater = getMenuInflater();
		inflater.inflate(R.menu.options_menu, menu);
		return true;
	}

	/* Handles item selections in menu
	 */
	public boolean onOptionsItemSelected(MenuItem item)
	{
		//add the animation for marker control panel
		Animation animation_in = AnimationUtils.loadAnimation(this,android.R.anim.fade_in);
		Animation animation_out = AnimationUtils.loadAnimation(this,android.R.anim.fade_out);
		
		switch (item.getItemId())
		{
		case R.id.search_item:
			if(mapLayout.indexOfChild(markerLayout)>0)
			{
			    markerLayout.startAnimation(animation_out);
			    mapLayout.removeViewAt(mapLayout.indexOfChild(markerLayout));
			}
			//turn off the marker so that user can drag the map
			toOverlay.setMarkerTrigger(false);
			fromOverlay.setMarkerTrigger(false);
			//start the search bar
			onSearchRequested();
			break;
		case R.id.marker_item:
			if(mapLayout.indexOfChild(markerLayout)<0)
			{
				marker_radiogroup.check(R.id.map_radio);
				toOverlay.setMarkerTrigger(false);
				fromOverlay.setMarkerTrigger(false);
				//start marker control panel
				markerLayout.startAnimation(animation_in);
				mapLayout.addView(markerLayout);
			}
			
			break;

		case R.id.satellite_subitem:
			mapView.setSatellite(true);
			mapView.setStreetView(false);
			mapView.setTraffic(false);
			break;
		case R.id.map_subitem:
			mapView.setSatellite(false);
			mapView.setStreetView(false);
			mapView.setTraffic(false);
			break;
			
		case R.id.info_item:
			if(mapLayout.indexOfChild(markerLayout)>0)
			{
			    markerLayout.startAnimation(animation_out);
			    mapLayout.removeViewAt(mapLayout.indexOfChild(markerLayout));
			}
			
			toOverlay.setMarkerTrigger(false);
			fromOverlay.setMarkerTrigger(false);
			//show up the instruction of dialog
			showDialog(INFO_DIALOG);
			break;
		case R.id.send_item:
			if(mapLayout.indexOfChild(markerLayout)>0)
			{
			    markerLayout.startAnimation(animation_out);
			    mapLayout.removeViewAt(mapLayout.indexOfChild(markerLayout));
			}
			toOverlay.setMarkerTrigger(false);
			fromOverlay.setMarkerTrigger(false);
			
			//send the request to server. if there is not marker on location, 
			//the program will send notification. 
			if(toOverlay.getAddress()==null||fromOverlay.getAddress()==null)
			{
				Toast.makeText(this, "From or To Locattion is missing", Toast.LENGTH_SHORT).show();
			}else
			{
				
				showDialog(SEND_REQUEST);
			}
			break;
		case R.id.cancel_item:
			//send the cancel request to server
			 progressDialog = ProgressDialog.show(TaxiShareActivity.this, "Sending","Please wait...", true);
			
			 //separate the progress dialog with GUI thread 
			 ProgressThread progressThread = new ProgressThread(handler,mobileNumber,"cancel");
		     progressThread.start();
			return true;
		}
		return false;
	}

	/*
	 * onClick() function for marker control panel
	 * @see android.view.View.OnClickListener#onClick(android.view.View)
	 */
	@Override
	public void onClick(View v)
	{
		switch(v.getId())
		{
			case R.id.from_radio:
				//if there is not marker existing in fromOverlay, add the new from marker
				if(fromOverlay.size()==0)
				{
					GeoPoint point = mapView.getMapCenter();
					OverlayItem overLayItem = new OverlayItem(point,"Source","Start Point");
					fromOverlay.addOverlayItem(overLayItem);
					mapOverlays.add(fromOverlay);
				}
		
				fromOverlay.setMarkerTrigger(true);
				toOverlay.setMarkerTrigger(false);
				mapView.invalidate();
				
				break;
			case R.id.to_radio:
				
				//if there is not marker existing in toOverlay, add the new To marker
				if(toOverlay.size()==0)
				{
					GeoPoint point = mapView.getMapCenter();
					OverlayItem overLayItem = new OverlayItem(point,"Destination","End Point");
					toOverlay.addOverlayItem(overLayItem);
					mapOverlays.add(toOverlay);
				}
				
				fromOverlay.setMarkerTrigger(false);
				toOverlay.setMarkerTrigger(true);
				mapView.invalidate();
				break;
			case R.id.map_radio:
				//switch to the map control so that user can drag the map
				toOverlay.setMarkerTrigger(false);
				fromOverlay.setMarkerTrigger(false);
				break;
			case R.id.reset_button:
				
				//clear the From or To marker from fromOverlay or toOverlay
				//reset all the address to null				
				marker_radiogroup.check(R.id.map_radio);
				fromOverlay.setMarkerTrigger(false);
				toOverlay.setMarkerTrigger(false);
				fromOverlay.removeOverlayItem();
				fromOverlay.setAddress(null);
				toOverlay.removeOverlayItem();
				toOverlay.setAddress(null);
				mapView.invalidate();
		}
	}
	
	/*
	 * Receive the keyword from search bar and Extra the query 
	 * into didYouMean() functions
	 * @see com.google.android.maps.MapActivity#onNewIntent(android.content.Intent)
	 */
	public void onNewIntent(final Intent newIntent)
	{
		super.onNewIntent(newIntent);

		// get and process search query here
		final String queryAction = newIntent.getAction();
		if (Intent.ACTION_SEARCH.equals(queryAction))
		{
			final String queryString = newIntent.getStringExtra(SearchManager.QUERY);
			didYouMean(queryString);
		} 
	}
	
	/*
	 * (non-Javadoc)
	 * @see com.google.android.maps.MapActivity#isRouteDisplayed()
	 */

	protected boolean isRouteDisplayed()
	{
		return false;
	}

	/*
	 *  create the activity dialog
	 *  called by showDialog() 
	 * @see android.app.Activity#onCreateDialog(int)
	 */
	protected Dialog onCreateDialog(int id) 
	{
	    Dialog dialog = null;
	    switch(id) 
	    {
	    	//send the request dialog
	    	case SEND_REQUEST:
	    		
			AlertDialog.Builder builderRequest = new AlertDialog.Builder(this);
			builderRequest.setIcon(R.drawable.alert_dialog_icon);
			builderRequest.setTitle(R.string.title_senddialog);
			builderRequest.setMessage(generateConfirmation());
			builderRequest.setPositiveButton(R.string.ok_dialog,
				new DialogInterface.OnClickListener()
				{

					public void onClick(DialogInterface dialog, int which)
					{
						//show the progress dialog
					    progressDialog = ProgressDialog.show(TaxiShareActivity.this, "Sending","Please wait...", true);
						
					    //separate the progreass dialog with GUI thread
					    
						ProgressThread progressThread = new ProgressThread(handler,fromLocation,toLocation,mobileNumber,"send");
				        progressThread.start();
					}
				});

			builderRequest.setNegativeButton(R.string.cancel_senddialog,
				new DialogInterface.OnClickListener()
				{
					public void onClick(DialogInterface dialog, int which)
					{
						dialog.dismiss();	
					}
				});

			dialog = builderRequest.create();
			
	    		
	    		break;
	    	case ENTER_NUM:
	    		//the dialog for enter the mobile number
	    		LayoutInflater factory = LayoutInflater.from(this);
	    		final View numDialogLayout = factory.inflate(R.layout.textdialoglayout, null);
	    		Button exitButton = (Button)numDialogLayout.findViewById(R.id.num_exit);
	    		Button enterButton =(Button)numDialogLayout.findViewById(R.id.num_enter);
	    		
	    		AlertDialog.Builder builderNum = new AlertDialog.Builder(this);
	    		builderNum.setIcon(R.drawable.alert_dialog_icon);
	    		builderNum.setTitle(R.string.title_numdialog);
	    		builderNum.setView(numDialogLayout);
	    		exitButton.setOnClickListener(new View.OnClickListener()
				{
					
					@Override
					public void onClick(View v)
					{
						TaxiShareActivity.this.finish();
					}
				});
	    		
	    		enterButton.setOnClickListener(new View.OnClickListener()
				{
					
					@Override
					public void onClick(View v)
					{
						EditText numEditText =(EditText) numDialogLayout.findViewById(R.id.num_edittext);
						CharSequence mNumber = numEditText.getText();
						//if number is empty, pop up the notification
						if(mNumber.toString().equals(""))
						{
							Toast.makeText(v.getContext(),"Please Enter your NUMBER" , Toast.LENGTH_SHORT).show();
						}else
						{
							mobileNumber=mNumber.toString();
							TaxiShareActivity.this.dismissDialog(ENTER_NUM);
						}
						
					}
				});
	    			
	    	
	    		dialog=builderNum.create();
	    		dialog.setCancelable(false);
	    		
	    		break;
	    	case CHECK_NUM:
	    		AlertDialog.Builder builderCheckNum = new AlertDialog.Builder(this);
	    		builderCheckNum.setIcon(R.drawable.alert_dialog_icon);
	    		builderCheckNum.setTitle(R.string.title_numcheck);
	    		builderCheckNum.setMessage(mobileNumber);
	    		builderCheckNum.setPositiveButton(R.string.dialog_yes, 
	    			new DialogInterface.OnClickListener()
					{

	    				public void onClick(DialogInterface dialog, int which)
	    				{
	    					mobileNumber=mobileNumber;
	    				}
					});
	    		
	    		builderCheckNum.setNegativeButton(R.string.dialog_no,
	    			new DialogInterface.OnClickListener()
					{

						public void onClick(DialogInterface dialog, int which)
						{
							showDialog(ENTER_NUM);
						}
					});
	    		
	    		dialog=builderCheckNum.create();
	    		dialog.setCancelable(false);
	    		break;
	    	
	    	case INFO_DIALOG:
	    		//information dialog show the instruction of the application
	    		AlertDialog.Builder builderInfo = new AlertDialog.Builder(this);
	    		builderInfo.setIcon(R.drawable.alert_dialog_icon);
	    		builderInfo.setTitle(R.string.title_help);
	    		builderInfo.setMessage(R.string.instruction);
	    		builderInfo.setNeutralButton(R.string.ok_dialog,
				new DialogInterface.OnClickListener()
				{

					public void onClick(DialogInterface dialog, int which)
					{
						dialog.dismiss();
					}
				});
	    		
	    		dialog = builderInfo.create();
	    		break;
	    	
	    	case NET_CONNECT:
	    		//check the internet connection
	    		AlertDialog.Builder builderNet = new AlertDialog.Builder(this);
	    		builderNet.setIcon(R.drawable.alert_dialog_icon);
	    		builderNet.setTitle("Internet Problem");
	    		builderNet.setMessage("Please check Internet Connection");
	    		builderNet.setNeutralButton(R.string.ok_dialog,
				new DialogInterface.OnClickListener()
				{

					public void onClick(DialogInterface dialog, int which)
					{
						TaxiShareActivity.this.finish();
					}
				});
	    		dialog= builderNet.create();
	    		dialog.setCancelable(false);
	    		break;
	    	default:
	    		dialog = null;
	    }
	    return dialog;
	}
	
	/*
	 * change the dialog information 
	 * @see android.app.Activity#onPrepareDialog(int, android.app.Dialog)
	 */
	protected void onPrepareDialog (int id, Dialog dialog)
	{
		switch(id)
		{
		case SEND_REQUEST:
			((AlertDialog)dialog).setMessage(generateConfirmation());
			break;
		}
	}
	
	/*
	 * didYouMean() to verify and modify the input location 
	 * provide the suggestion
	 */
	public boolean didYouMean(String locationName)
	{
		Geocoder geoCoder = new Geocoder(this,Locale.getDefault());
		String locationMsg="";
		ArrayList<String> locationList= new ArrayList<String>();
		final CharSequence[] items;
		
		//check the location by geocoder
		try
		{
			//receive the maximum 5, suggestion from google geocoder service
			List<Address> addresses = geoCoder.getFromLocationName(locationName, 5);
			
			if(addresses.size()>0)
			{
				for(Address address: addresses)
				{
					locationMsg="";
					for (int i = 0; i <= address.getMaxAddressLineIndex(); i++)
						locationMsg += address.getAddressLine(i)+" ";
			
					//try to avoid duplicate suggestion
					if(!locationList.contains(locationMsg))
						locationList.add(locationMsg);
				}
			}else
			{
				//if no result show, no dailog
				Toast.makeText(this, "The location doesn't exist!", Toast.LENGTH_SHORT).show();
				return false;
			}
			
		} catch (IOException e)
		{
			//handler the geocoder service problem
			Log.i("debug",e.toString());
		}
		
		//if the result is same as suggestion, no suggestion dialog.
		if(locationList.size()==1 && locationList.get(0).toLowerCase().equals(locationName.toLowerCase()+" "))
		{
			animateToGeoPoint(locationName);
			return false;
		}
		
		//convert charsequence array to string array
		Object[] array =locationList.toArray();
		items = new CharSequence[array.length];
		for(int i=0; i<locationList.size(); i++)
		{
			items[i]= array[i].toString();
		}
		//pop up the dialog
		AlertDialog alert = new AlertDialog.Builder(this)
        .setTitle("Did you mean")
        .setItems(items, new DialogInterface.OnClickListener() 
        {
            public void onClick(DialogInterface dialog, int which) 
            {
            	animateToGeoPoint(items[which].toString());
            	
            }
        }).create();
		
		alert.show();
		return true;
	}
	
	
	/*
	 * filter the information and generation the confirmation
	 */
	public String generateConfirmation()
	{
		String from = "From:\n";
		String templocal;
		for (int i = 0; i < fromOverlay.getAddress().getMaxAddressLineIndex(); i++)
		{
			from += fromOverlay.getAddress().getAddressLine(i) + "\n";
		}
		
		// simplify the information from detail location information
		if(fromOverlay.getAddress().getThoroughfare()==null)
		{
			fromLocation = fromOverlay.getAddress().getLocality();
		}else
		{	
			templocal=fromOverlay.getAddress().getThoroughfare().replaceAll("\\b\\s\\S+\\b$", "");
		
			if(templocal.equals(fromOverlay.getAddress().getLocality())||fromOverlay.getAddress().getLocality().contains(templocal))
			{
				fromLocation = fromOverlay.getAddress().getLocality();
			}else
			{
				fromLocation=templocal+" "+fromOverlay.getAddress().getLocality();
			}
		}
		
		String to = "To:\n";
		for (int i = 0; i < toOverlay.getAddress().getMaxAddressLineIndex(); i++)
		{
			to += toOverlay.getAddress().getAddressLine(i) + "\n";
		}
		
		//simplify the information from detail destination location information
	    if(toOverlay.getAddress().getThoroughfare()==null)
	    {
	    	toLocation=toOverlay.getAddress().getLocality();
	    }else
	    {
		
	    	templocal=toOverlay.getAddress().getThoroughfare().replaceAll("\\b\\s\\S+\\b$", ""); 
		
	    	if(templocal.equals(toOverlay.getAddress().getLocality())||toOverlay.getAddress().getLocality().contains(templocal))
	    	{
	    		toLocation = toOverlay.getAddress().getLocality();
	    	}else
	    	{
	    		toLocation=templocal+" "+toOverlay.getAddress().getLocality();
	    	}
	    }
		
		to+=toOverlay.getAddress().getLocality()+"\n";
		
		
//		Log.i(tag, "from: "+fromOverlay.getAddress().getLocality()+" To: "+toOverlay.getAddress().getLocality());
		
		return from + "\n" + to;
	}
	
	//animate to move to a location
	public void animateToGeoPoint(String locationName)
	{
		Geocoder geoCoder = new Geocoder(this,Locale.getDefault());
    	GeoPoint geoPoint;
    	int latitude=0;
    	int longitude=0;
    	try
		{
			List<Address> addresses = geoCoder.getFromLocationName(locationName, 1);
			latitude=(int)(addresses.get(0).getLatitude()*1E6);
			longitude=(int)(addresses.get(0).getLongitude()*1E6);
			geoPoint = new GeoPoint(latitude,longitude);

			mapController.animateTo(geoPoint);
			mapController.setZoom(15);
			mapView.invalidate();
			
		} catch (IOException e)
		{
			e.printStackTrace();
		}
	}

	//handler for dismiss the progress dialog
	final Handler handler = new Handler() 
	{
        public void handleMessage(Message msg) 
        {
        	progressDialog.dismiss();
        	
        }
        
    };
	
	
	private class ProgressThread extends Thread 
    {
        Handler mHandler;
        String source;
        String destination;
        String mobileNum;
        String type;
    
        //the construct for send request
        ProgressThread(Handler h,String source, String destination, String mobileNum, String type)
        {
            mHandler = h;
            this.source=source;
            this.destination=destination;
            this.mobileNum=mobileNum;
            this.type=type;
        }
        
        //the construct for cancel request
        ProgressThread(Handler h,String mobileNum,String type)
        {
            mHandler = h;
            this.mobileNum=mobileNum;
            this.type=type;
        }
        
      
        public void run() 
        {
        	HttpConnection httpConnection = new HttpConnection();
        	
        	
        	if(type.equals("send"))
        	{
        		httpConnection.httpConnect(source, destination, mobileNum);
        	}else if(type.equals("cancel"))
        	{
        		httpConnection.sendCancel(mobileNum);
        	}
        	
            Message msg = mHandler.obtainMessage();
            Bundle b = new Bundle();
            b.putString("Result","con");
            msg.setData(b);
            mHandler.sendMessage(msg);
            
        }
        
    }
	
}