<?php
	include "conn.php";
	include "allfunctions.php";
	  session_start();
	  $ownerCD=$_POST['OWNER_CD'];
	  $tag=$_POST['tag'];
	  // {"time":getCurtime(),"chassisid":chassisid,"bladeid":bladeid,"transmit":transmit,"portid":portid,"ptype":ptype}        
	 $chassisid=$_POST['chassisid'];
	 $bladeid=$_POST['bladeid'];
	 $transmit=strtoupper($_POST['transmit']);

	 $portid=$_POST['portid'];
	 $ptype=$_POST['ptype'];
	 $rackid=$_POST['rack_number'];	 
	 	 $rackkey=$_POST['rack_key'];	
	 $uid=$_POST['uid']; 
	 $time=$_POST['time'];
	 $lblid=$_POST['labelid'];
	 

	 $ownerky=getOwnerkey($ownerCD);
	 $sitekey=getHostkey($tag);

	 $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'port'";
        $autoquery=mysqli_query($con,$autoidquery);
        $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT'];

	 $sql="insert into port(OWNER_CD, site, rack, chassis, slot_or_blade_number,created_on, created_by,`key`,record_state,owner_key,site_key,chassis_key,slot_key,port_number,type,xmt_rcv,rack_key,created_from,port_label) values('$ownerCD','$tag','$rackid','$chassisid','$bladeid','$time','$uid','$autoid','L','$ownerky','$sitekey','$chassisid','$bladeid','$portid','$ptype','$transmit','$rackkey','$createdfrm','$lblid')";

	 $sqls="select * from `port` where chassis_key='$chassisid' and slot_key='$bladeid' and port_number='$portid' and record_state='L'";
     $ress= mysqli_query($con,$sqls);	
	 if(mysqli_num_rows($ress)>0)	{
		 $ures=mysqli_fetch_assoc($ress);
		 $rid=$ures['id'];
		 $rky=$ures['key'];
		$sqlu="update port set record_state='H',updated_on='$time',updated_from='$createdfrm',updated_by='$uid' where `id`='$rid'";
		$resu=mysqli_query($con,$sqlu);
		$sql="insert into port(OWNER_CD, site, rack, chassis, slot_or_blade_number,created_on, created_by,`key`,record_state,owner_key,site_key,chassis_key,slot_key,port_number,type,xmt_rcv,rack_key,created_from,port_label) values('$ownerCD','$tag','$rackid','$chassisid','$bladeid','$time','$uid','$rky','L','$ownerky','$sitekey','$chassisid','$bladeid','$portid','$ptype','$transmit','$rackkey','$createdfrm','$lblid')";
		if($resu)
			$res=mysqli_query($con,$sql);      		 
	} // if(mysqli_num_rows($ress)>0)	
	else{
       $res=mysqli_query($con,$sql);
	 } // else
	if($res){
		echo "1";
	} else {
		echo "0";
	}

?>