<?php
	include "conn.php";

	 
	$job=$_POST['jobnum'];
	$jobkey=$_POST['jobkey'];
	//$empid=$_POST['employee_id'];	
	$lat=$_POST['lattitude2'];
	$lon=$_POST['longitude2'];
	$job_phase_number = $_POST['job_phase'];

	$per_diem=$_POST['per_diem'];
	$event_type=$_POST['ev_type'];
	
	$uid=$_POST['uid'];
	$employeeid=$_POST['employeeid'];
	$leader="L";

	  $event_time =$_POST['time'];
	  if(isset($_POST['hr']) && $_POST['hr']!="")
	  $hr =$_POST['hr'];
	  if(isset($_POST['min']) && $_POST['min']!="")
	  $min =$_POST['min'];


	  //if(isset($_POST['hr']) && $_POST['hr']!="" && isset($_POST['min']) && $_POST['min']!="") ){
	  $sql="insert into timesheet (job_key,employee,crew_leader,event_time,event_type,
	  latitude, longitude,job_phase_number, 
	  user_hour,user_minute,per_diem,
	  employee_approved,per_diem_approved) values('$jobkey','$employeeid','$uid','$event_time','$event_type',
	  '$lat','$lon','$job_phase_number',
	  '$hr','$min','$per_diem',
	  '$event_time','0')";
	  $res = mysqli_query($con,$sql);

	  //}
	  echo("OK ".$employeeid.$event_type.$event_time.$job);

?>
