<?php
	include "conn.php";
	include "allfunctions.php";
	#session_start();
  		$ownerCD=$_POST['OWNER_CD'];
  		$tag=$_POST['tag'];
        
	 $torack=$_POST['torack'];
	 $fromrack=$_POST['fromrack'];
	 
	 $fromchassis=$_POST['fromchassis'];
	 $tochassis=$_POST['tochassis'];
	 $fromblade=$_POST['fromblade'];
	 $toblade=$_POST['toblade'];
	 $fromport=$_POST['fromport'];
	 $toport=$_POST['toport'];
	 $jumperlen=$_POST['jumperlen'];
	  if(isset($_POST['stage'])) 
         $stage=$_POST['stage'];
         else
         $stage="0";
	 //$portnumber=$_POST['portnumber'];
	 //$porttype=$_POST['porttype'];
	 $time=$_POST['time'];
	 $uid=$_POST['uid']; 
	 $ownerky=getOwnerkey($ownerCD);
	 $sitekey=getHostkey($tag);
	 $jobkey=$_POST['jobkey'];
	 $actsts=$_POST['actsts'];

	 $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'jumper'";
     $autoquery=mysqli_query($con,$autoidquery);
     $keyassoc=mysqli_fetch_assoc($autoquery);
     $autoid=$keyassoc['AUTO_INCREMENT'];
	 $sqlinsert="insert into jumper(`key`, record_state, owner_key, site_key, job_key,from_rack_number_key, from_chassis_key, from_slot_key, from_port_key, to_rack_number_key, to_chassis_key, to_slot_key, to_port, length,created_on, created_by,stage) values('$autoid','L','$ownerky','$sitekey','$jobkey','$fromrack','$fromchassis','$fromblade','$fromport','$torack','$tochassis','$toblade','$toport','$jumperlen','$time','$uid','$stage')";
	 
	 if($actsts == "UPDATE")
	 {


	     $sqlsearchport2 = "SELECT `id` FROM jumper where site_key='$sitekey' AND record_state = 'L' AND from_port_key='$fromport' AND  to_port = '$toport'";
	 $resquery = mysqli_query($sqlsearchport, $conn);
	 if(mysqli_num_rows($resquery)>0){	 		 
		 $data=mysqli_fetch_assoc($resquery);
		 $rid = $data['id'];
	 	 $updatesql="update jumper set record_state='H',updated_on='$time',updated_from='collector apps',updated_by='$uid' where `id`='$rid'";
	 	 $resquery = mysqli_query($updatesql, $conn);
	 	}		

	  if(isset($_POST['uid'])){
		$res=mysqli_query($con,$sqlinsert);

		if($res){
			echo "1";
		} else {
			echo "0";
		}
	  } else{
	  	echo "0";
	  }
	}
	else if($actsts == "CHECK"){
	 $sqlsearchport = "SELECT `id` FROM jumper where site_key='$sitekey' AND record_state = 'L' AND (from_port_key='$fromport' OR to_port = '$toport')";

	 $resquery = mysqli_query($sqlsearchport, $conn);
	 if(mysqli_num_rows($resquery)>0){
	 	echo "3";
	 }		
	 else{
	 	$res=mysqli_query($con,$sqlinsert);
		if($res){
			echo "1";
		} else {
			echo "0";
		}
	 }

	}

?>