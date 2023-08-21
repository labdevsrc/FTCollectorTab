<?php
	include "conn.php";

	$employeeid=$_POST['employeeid'];	 
	$equipment_id=$_POST['equipment_id'];		
	$job=$_POST['jobnum'];
	$job_phase_number = $_POST['job_phase'];
	$date_out =$_POST['date_out'];
	$date_returned =$_POST['date_returned'];
	$hours_or_miles_out=$_POST['hours_or_miles_out'];
	$hours_or_miles_in=$_POST['hours_or_miles_in'];



	if($date_out == "0" && $hours_or_miles_out == "0")
		$isinsert = false;
	else if($date_returned == "0" && $hours_or_miles_in == "0")
		$isinsert = true;

	$checkrow="select max(id) MaxID, job_equipment.* from job_equipment where employee='$employeeid' and job = '$job'
			and equipment_id = '$equipment_id' and hours_or_miles_in='0'";
	$result = mysqli_query($con,$checkrow); 
	echo("mysqli_num_rows(result) ".mysqli_num_rows($result));

	if($isinsert)
	{
	  $sql="insert into job_equipment (job,employee,equipment_id,date_out,date_returned,
	  job_phase_number, hours_or_miles_out,hours_or_miles_in) 
		values('$job','$employeeid','$equipment_id','$date_out','$date_returned',
		'$job_phase_number','$hours_or_miles_out','$hours_or_miles_in')";
	  $res = mysqli_query($con,$sql);
	  echo("insert Equip CHECKOUT, maxid ".mysqli_num_rows($result)." ".$employeeid." ".$equipment_id." ".$date_out." ".$hours_or_miles_out);
	}		
	else{
	   if(mysqli_num_rows($result))
	   {

		//$row=mysqli_fetch_assoc($result);

		$row=mysqli_fetch_array($result);
		$maxid= $row["MaxID"];	
		echo("UPDATE Equip CHECKOUT maxid : ".$maxid);		
		//$date_from_apk = new DateTime($date_out);
		//$db_dateout = new DateTime($row["date_out"]);
		//$interval = $db_dateout->diff($date_from_apk); // check time difference from equipment checkout to equipment checkin		

		$update = "UPDATE job_equipment SET date_returned ='$date_returned', hours_or_miles_in='$hours_or_miles_in' where id=$maxid";
		$res_update= mysqli_query($con,$update );		
		//echo("maxid ".$maxid." , date_out ".$row["date_out"].", interval ".$interval->format('%R%a days'));	   
		}		
	}

?>