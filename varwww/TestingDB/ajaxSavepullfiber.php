<?php
	include "conn.php";
	include 'allfunctions.php';
	session_start();
	//time":getCurtime(),"cable":cable,"pout":pout,"pin":pin
	$time=$_POST['time'];
	$cable=$_POST['cable_id_key'];
	$pout=$_POST['to_tagnumber'];
	$pin=$_POST['from_tagnumber'];
	$totallength=$_POST['totallength'];
	$ductout=$_POST['to_duct'];
	$ductin=$_POST['from_duct'];
	$cabletype=$_POST['cabletype'];
	$install=$_POST['install'];
	$sheathout=$_POST['sheathout'];
	$sheathin=$_POST['sheathin'];

	
	//sessions
	$ownerCD=$_POST['OWNER_CD'];
    $jno=$_POST['jobnum'];
    $uid=$_POST['uid'];
    $stage=$_POST['stage'];

     /// Get values from Site for from site
    $sqlfromtag="select * from Site  where tag_number='$pout' and record_state='L' and created_by='$uid' and job='$jno'";
	$resfromtag= mysqli_query($con,$sqlfromtag);	
	$rowfromtag=mysqli_fetch_assoc($resfromtag);
	$fromtag_number=$rowfromtag['id'];
	/////////////////////
	   /// Get values to Site for from site
    $sqltotag="select * from Site  where tag_number='$pin' and record_state='L' and created_by='$uid' and job='$jno'";
	$restotag= mysqli_query($con,$sqltotag);	
	$rowtotag=mysqli_fetch_assoc($restotag);
	$totag_number=$rowtotag['id'];
	/////////////////////

    /// Get values from CONDUITS_GROUP for from duct
    $sqlduct1="select * from CONDUITS_GROUP where OWNER_CD='$ownerCD' and host_tag_number='$pout' and created_by='$uid' and record_state='L'";
	$resduct1= mysqli_query($con,$sqlduct1);	
	$rowduct1=mysqli_fetch_assoc($resduct1);
	$from_duct=$rowduct1['id'];
	//and type='',
    /////
     /// Get values from CONDUITS_GROUP for to duct
    $sqlduct2="select * from CONDUITS_GROUP where OWNER_CD='$ownerCD' and host_tag_number='$pin' and created_by='$uid' and record_state='L'";
	$resduct2= mysqli_query($con,$sqlduct2);	
	$rowduct2=mysqli_fetch_assoc($resduct2);
	$to_duct=$rowduct2['id'];
	//and type='',
    /////
    //$id = mysqli_insert_id();
      ////*******Getting owner key******
  $ownerkeysql="select * from `owner` where OWNER_CD='$ownerCD' and record_state='L'";
  $resoquery=mysqli_query($con,$ownerkeysql);
  $resokey=mysqli_fetch_assoc($resoquery);
  $ownerkey=$resokey['key'];
    $qr="SELECT `AUTO_INCREMENT` akey FROM  INFORMATION_SCHEMA.TABLES WHERE `TABLE_SCHEMA` = '$database' AND   `TABLE_NAME`   = 'a_fiber_segment'";
    $qry=mysqli_query($con,$qr);
    $mysqlres=mysqli_fetch_assoc($qry);
	$keyval=$mysqlres['akey'];
	$poutkey=$pout;
	$pinkey=$pin;
	$pout=gettagnumber($pout);
	$pin=gettagnumber($pin);
	$jkey=$_SESSION['jobkey'];
	//
	//from_site_duct_direction,from_site_duct_direction_count,sheath_out,to_site_duct_direction,to_site_duct_direction_count,sheath_in
	$ductdet1="select * from CONDUITS_GROUP where host_tag_number='$pout' and record_state='L'";
    $ductqr1=mysqli_query($con,$ductdet1);
    $ductres1=mysqli_fetch_assoc($ductqr1);
    $from_site_duct_direction=$ductres1['direction'];
    $from_site_duct_direction_count=$ductres1['direction_count'];
    //
    $ductdet2="select * from CONDUITS_GROUP where host_tag_number='$pin' and record_state='L'";
    $ductqr2=mysqli_query($con,$ductdet2);
    $ductres2=mysqli_fetch_assoc($ductqr2);
    $to_site_duct_direction=$ductres2['direction'];
    $to_site_duct_direction_count=$ductres2['direction_count'];
    

    //$ownerKey=$_SESSION['ownerkey'];
	$sql="insert into a_fiber_segment (`OWNER_CD`,`cable_id`,`cable_id_key`,`job`,`stage`,`from_site`,`from_site_key`,`to_site`,`to_site_key`,`created_on`,`created_by`,`cable_length`,`from_site_duct`,`from_site_duct_key`,`to_site_duct`,`to_site_duct_key`,`key`,`cable_type`,`owner_key`,`install_method`,job_key,from_site_duct_direction,from_site_duct_direction_count,sheath_out,to_site_duct_direction,to_site_duct_direction_count,sheath_in,created_from) values('$ownerCD','$cable','$cable','$jno','$stage','$pout','$poutkey','$pin','$pinkey','$time','$uid','$totallength','$ductout','$ductout','$ductin','$ductin','$keyval','$cabletype','$ownerkey','$install','$jkey','$from_site_duct_direction','$from_site_duct_direction_count','$sheathout','$to_site_duct_direction','$to_site_duct_direction_count','$sheathin','field collection')";
	//'$fromtag_number','$totag_number'
	$res= mysqli_query($con,$sql);	
	if($res){
		echo "1";
	} else {
		echo "0";
	}	
	//echo $pin.$sql.$res.$sqlduct2;
	//echo json_encode($data);
	
?>

