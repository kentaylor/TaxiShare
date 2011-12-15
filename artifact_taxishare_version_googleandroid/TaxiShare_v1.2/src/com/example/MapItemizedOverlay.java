package com.example;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import android.content.Context;
import android.graphics.Point;
import android.graphics.drawable.Drawable;
import android.location.Address;
import android.location.Geocoder;
import android.util.Log;
import android.view.MotionEvent;
import android.widget.Toast;

import com.google.android.maps.GeoPoint;
import com.google.android.maps.ItemizedOverlay;
import com.google.android.maps.MapView;
import com.google.android.maps.OverlayItem;
/*
 * MapItemizedOverlay to contain the marker and manipulate the location
 */
public class MapItemizedOverlay extends ItemizedOverlay
{
	public final static String tag="debug";
	private ArrayList<OverlayItem> overlayItems = new ArrayList<OverlayItem>();
	private Address address=null;
	private boolean markerTrigger=false;
	
	public MapItemizedOverlay(Drawable defaultMarker)
	{
		super(boundCenterBottom(defaultMarker));
	}

	//if markerTrigger is activate, and the marker can be manipulated
	public void setMarkerTrigger(boolean trigger)
	{
		markerTrigger=trigger;
	}
	
	public boolean getMarkerTrigger()
	{
		return markerTrigger;
	}
	
	public Address getAddress()
	{
		return address;
	}
	
	public void setAddress(Address address)
	{
		this.address=address;
	}

	/*
	 * create the item
	 * @see com.google.android.maps.ItemizedOverlay#createItem(int)
	 */
	@Override
	protected OverlayItem createItem(int i)
	{
		return overlayItems.get(i);
	}

	/*
	 * get the size of overlayItems
	 * @see com.google.android.maps.ItemizedOverlay#size()
	 */
	@Override
	public int size()
	{
		return overlayItems.size();
	}

	public void addOverlayItem(OverlayItem item)
	{
		overlayItems.add(item);
		populate();
	}
	
	
	public void removeOverlayItem()
	{
		overlayItems.clear();
	}
	
	public boolean onTouchEvent(MotionEvent event, MapView mapView)
	{
		//when the finger release from screen, show up the location name
		if(event.getAction()==MotionEvent.ACTION_UP&& markerTrigger==true)
		{
			Geocoder geoCoder = new Geocoder(mapView.getContext());
			GeoPoint p = mapView.getProjection().fromPixels((int) event.getX(),(int) event.getY());
			String locationMsg="";
			try
			{
				List<Address> addresses=geoCoder.getFromLocation(p.getLatitudeE6()/1E6, p.getLongitudeE6()/1E6, 1);
				if(addresses.size()>0)
				{
					address= addresses.get(0);
					for(int i=0; i<address.getMaxAddressLineIndex();i++)
					{
						locationMsg+=addresses.get(0).getAddressLine(i)+"\n";
					}
				}
			} catch (IOException e)
			{
				Log.i(tag, e.toString());
			}
			
			Toast.makeText(mapView.getContext(), locationMsg, Toast.LENGTH_SHORT).show();
			//drag the marker around the map
		}else if (event.getAction() == MotionEvent.ACTION_MOVE && markerTrigger==true)
		{
			
			event.setAction(MotionEvent.ACTION_CANCEL);
			mapView.onTouchEvent(event);
			
			GeoPoint p = mapView.getProjection().fromPixels((int) event.getX(),
					(int) event.getY());
			OverlayItem item;
			if(overlayItems.get(0).getTitle().equals("Source"))
			{
				item = new OverlayItem(p,"Source","Start point");
			}else
			{
				item = new OverlayItem(p,"Destination","End point");
			}
			removeOverlayItem();
			addOverlayItem(item);
		}
		
		return false;
	}

}