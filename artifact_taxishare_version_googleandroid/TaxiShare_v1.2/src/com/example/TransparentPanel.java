package com.example;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.graphics.RectF;
import android.graphics.Paint.Style;
import android.util.AttributeSet;
import android.widget.LinearLayout;

/*
 * this Panel for marker panel 
 */
public class TransparentPanel extends LinearLayout
{
	Paint innerPaint;
	Paint borderPaint;
	public TransparentPanel(Context context, AttributeSet attrs)
	{
		super(context, attrs);
	}


	public TransparentPanel(Context context)
	{
		super(context);
		
		
	}

	protected void dispatchDraw(Canvas canvas) {
	
		innerPaint= new Paint();
		innerPaint.setARGB(225, 75, 75, 75);
		
		borderPaint = new Paint();
		borderPaint.setARGB(255, 255, 255, 255);
		borderPaint.setAntiAlias(true);
		borderPaint.setStyle(Style.STROKE);
		borderPaint.setStrokeWidth(2);
		
		RectF drawRect = new RectF();
		drawRect.set(0,0, getMeasuredWidth(), getMeasuredHeight());

		canvas.drawRoundRect(drawRect, 5, 5, innerPaint);
		canvas.drawRoundRect(drawRect, 5, 5, borderPaint);

		super.dispatchDraw(canvas);

	}
}
