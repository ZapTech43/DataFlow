<include>
	lib.so
	lib.databarrier
	lib.lexusima
	lib.networkingpolice
	lib.cocos2d
	lib.msxdft-41
import clock, dev, os, base64, time, datetime
	if :
	   {at.DataFlow.Framework.AppActivity.Controller = 1}
	then :
	     import network
	     find ipaddress, macaddress
	     copy ipaddress, macaddress to http://DataFlow.org/Controller/User/<number> ///Number will generated randomly :u

///DO NOT MODIFY THIS SECTION THIS SECTION IS FOR THE IMPORTANT BASE FOR THE PROGRAM EDIT IT IF YOU ARE ALLOWED!

	.{at.DataFlow.Framework.Engine}/dir={/master/EngineBase.dll}
	.{at.DataFlow.Framework.Engine2}/dir={/master/FlowController.dll}
	.{at.DataFlow.Resources}/dir={/master/Resources}
	.{at.DataFlow.Config.Settings}/dir={/master/settings.cfg}
	.{at.DataFlow.Config.Language}/dir={/master/language.cfg}
	.{at.DataFlow.Framework.AppActivity}/dir={/master/engines/Activity.dll}
	.{at.DataFlow.Framework.AppActivity.Controller.Engine}/dir={/master/engines/Controller21.dll}

///THIS CODE IS TO STOP THE PROGRAM FROM RUNNING IF THE CONTROLLER'S ENGINE CANNOT BE FOUND
	try :
	    import {at.DataFlow.Framework.AppActivity.Controller.Engine/Status}
	if :
	   {at.DataFlow.Framework.AppActivity.Controller.Engine = 0}
	then :
	   <Abort> 
		run {at.DataFlow.Framework.Engine}
		run {at.DataFlow.Framework.Engine2}
	   <Notify>
		  "[Error] Cannot Find The DataFlow Controller Engine Reinstalling it may fix the problem
		   Error Code : 0x00000007b" 
		   1-b,2-b,3-c
	set {at.DataFlow.Framework.AppActivity = 0}
\\<AbortOperation>//

///THIS CODE IS TO RUN THE PROGRAM

        try :
	    import {at.DataFlow.Framework.AppActivity.Controller.Engine/Status}
	if :
	   {at.DataFlow.Framework.AppActivity.Controller.Engine = 1}
	then :
	<start>
	     run {at.DataFlow.Framework.Engine}
	     run {at.DataFlow.Framework.Engine2}
	     run {at.DataFlow.Framework.AppActivity.Controller.Engine}

///THIS CODE IS TO MANAGE THE GUI

	import screenres, refreshrate, color
	copy screenres, refreshrate, color as <screenres-refreshrate-color>
	if :
	   <1366x768-60hz-256>
	then :
	     draw :
	          top  = /master/Resources/layoutTopN.xml
	          fore = /master/Resources/layoutForeN.xml	
	then :
	     run {DataFlow.exe}

	if :
	   <1440x900-60hz-256>
	then :	
	     draw :
	          top  = /master/Resources/layoutTopN.xml
	          fore = /master/Resources/layoutForeN.xml
	then :
	     run {DataFlow.exe}

	if :
	   <1920x1080-60hz-256>
	then :
	     draw :
	          top  = /master/Resources/layoutTopHD.xml
	          fore = /master/Resources/layoutForeHD.xml

///THIS CODE IS RESPONSIBLE FOR THE CONTROLLER TASK

	import priority, bandwith, downspeed, upspeed from {at.DataFlow.Framework.AppActivity.Collector}
	copy ipaddress
	
	set <priority> as <Importantwantings>
	then :
	     export priority 
	<start>
	     run {at.DataFlow.Framework.Engine.Prioritizer}
	     run {at.DataFlow.Framework.Engine2.Prioritizer}
	     run {at.DataFlow.Framework.AppActivity.Controller.Engine}
	set {at.DataFlow.Framework.AppActivity.Controller.Engine} as {at.DataFlow.Framework.AppActivity.Controller.Engine+(Priority)}
	then :
	     set nettrafficreceiver as controllednettrafficreceiver to {at.DataFlow.Framework.AppActivity.Controller}
	     import nettrafficreceiver, downspeed, upspeed
	if :
	   nettrafficreceiver = <priorityname>
	   downspeed = <1000 
	   upspeed   = <50
	then :
	     stop {at.DataFlow.Framework.AppActivity.Controller.Router}
	     stop {at.DataFlow.Framework.AppActivity.Controller.Blocker}
	     stop {at.DataFlow.Framework.AppActivity.Controller.Crasher}
	     stop {at.DataFlow.Framework.AppActivity.Controller.Delayer}
	if :
	   nettrafficreceiver = <priorityname>
	   downspeed = >1000 
	   upspeed   = >50
	then :
	     start {at.DataFlow.Framework.AppActivity.Controller.Router}
	     start {at.DataFlow.Framework.AppActivity.Controller.Blocker}
	     start {at.DataFlow.Framework.AppActivity.Controller.Crasher}
	     start {at.DataFlow.Framework.AppActivity.Controller.Delayer}

///THIS CODE IS RESPONSIBLE FOR THE AUTO PRIORITIZER TASK
///AUTO PRIORITIZER IS USEFUL FOR MULTI-TASKER SO THAT AUTO PRIORITIZER CAN PRIORITIZE 3-5 APPS THAT USES INTERNET BUT IT DEPENDS ON USER'S MOST OFTEN ACTIVITY
///BECAUSE IF USER MANUALLY PRIORITIZE THE TRAFFIC INTO ONE APP THAT CAN CAUSE OTHER APP EVEN THE OS UNSTABLE

	import taskman, nettraffic, 
	copy taskman as NetTask for {at.DataFlow.Framework.Engine.AutoPrioritizer}

	find :
	     {InternetActivity.Download}
	     {InternetActivity.Upload}
	     {InternetActivity.Traffic}
	     {InternetActivity.Usage}
	then :
	     import nettraffic, downspeed, upspeed as netreport.txt 
	     send netreport.txt to {at.DataFlow.Framework.AppActivity.Prioritizer}
	if :
	   {at.DataFlow.Framework.AppActivity.Prioritizer = 1}
	   {at.DataFlow.Framework.AppActivity.AutoPrioritizer = 0}
	then :
	     \\<BreakOperation>//
	     start {at.DataFlow.Framework.AppActivity.AutoPrioritizer}
	     start {at.DataFlow.Framework.AppActivity.AutoPrioritizer.PriorityFinder}
	     start {at.DataFlow.Framework.AppActivity.AutoPrioritizer.TaskManagerPrioritizer}
	     import nettraffic, downspeed, upspeed as netreport.txt 
	     send netreport.txt to {at.DataFlow.Framework.AppActivity.Prioritizer}
but
	if :
	   {at.DataFlow.Framework.AppActivity.Prioritizer = 0}
	   {at.DataFlow.Framework.AppActivity.AutoPrioritizer = 1}
	then :
	     import taskmanlist, downspeed, nettraffic as netreport.txt
	     send netreport.txt to {at.DataFlow.Framework.AppActivity.Prioritizer}
	then :
	     start {at.DataFlow.Framework.AppActivity.AutoPrioritizer}
	     start {at.DataFlow.Framework.AppActivity.AutoPrioritizer.PriorityFinder}
	     start {at.DataFlow.Framework.AppActivity.AutoPrioritizer.TaskManagerPrioritizer}
	     <Notify>
		    "Successful!, Let Us Choose The Best For You!"
	if :
	   {at.DataFlow.Framework.AppActivity.Prioritizer = 0}
	   {at.DataFlow.Framework.AppActivity.AutoPrioritizer = 2}
	then :
	     start {at.DataFlow.Framework.AppActivity.AutoPrioritizer.Router}
	     start {at.DataFlow.Framework.AppActivity.AutoPrioritizer.Blocker}
	     start {at.DataFlow.Framework.AppActivity.AutoPrioritizer.Crasher}
	     start {at.DataFlow.Framework.AppActivity.AutoPrioritizer.Delayer}
	     import prioritizedlist from {at.DataFlow.Framework.AppActivity.AutoPrioritizer.PriorityFinder}
	     <Notify>
	   	    "Here's What We Did Prioritize Do You Agree?"
	if :
	   {at.DataFlow.Framework.AppActivity.AutoPrioritizer.PriorityFinder = 0}
	then : 
	     \\<BreakOperation>//
	     stop {at.DataFlow.Framework.AppActivity.AutoPrioritizer.Router}
	     stop {at.DataFlow.Framework.AppActivity.AutoPrioritizer.Blocker}
	     stop {at.DataFlow.Framework.AppActivity.AutoPrioritizer.Crasher}
	     stop {at.DataFlow.Framework.AppActivity.AutoPrioritizer.Delayer}
	restart :
	        {at.DataFlow.Framework.AppActivity.AutoPrioritizer}
	        {at.DataFlow.Framework.AppActivity.AutoPrioritizer.PriorityFinder}
	        {at.DataFlow.Framework.AppActivity.AutoPrioritizer.TaskManagerPrioritizer}
	     <Notify>
		    "Okay We'll Try To Prioritizing Again Sorry! :D"
but
	if :
	   {at.DataFlow.Framework.AppActivity.AutoPrioritizer.PriorityFinder = 1}
	then : 
	     stop  {at.DataFlow.Framework.AppActivity.AutoPrioritizer.PriorityFinder}
	     stop  {at.DataFlow.Framework.AppActivity.AutoPrioritizer.TaskManagerPrioritizer}
	     start {at.DataFlow.Framework.AppActivity.AutoPrioritizer.Router}
	     start {at.DataFlow.Framework.AppActivity.AutoPrioritizer.Blocker}
	     start {at.DataFlow.Framework.AppActivity.AutoPrioritizer.Crasher}
	     start {at.DataFlow.Framework.AppActivity.AutoPrioritizer.Delayer}
	     <Notify>
	            "Thank You Sir!, Now We'll Prioritize This Apps! :D"
	
