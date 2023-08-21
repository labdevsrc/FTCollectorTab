	<?php
	session_start();
	include "conn.php";		
  include "allfunctions.php";
  $time=$_POST['time'];
	$chskey=$_POST['chassis_key'];  
  $bnumber=$_POST['slot_or_blade_number'];  
  $mfr=$_POST['manufacturer_key'];
  $mod=$_POST['model_key'];
  $port=$_POST['port'];  
  $uid=$_POST['uid'];
  $rnumber=$_POST['rack_number'];
  $rkey=$_POST['rack_key'];
   if(isset($_POST['stage'])) 
         $stage=$_POST['stage'];
         else
         $stage="0";
  //$ownerCD=$_SESSION['oid'];
  $siteid=$_POST['tag'];
  $owner_cd=$_POST['OWNER_CD']; 
  //4   
   
    $msg= array("d" => "Saved...");

if(isset($_POST['chasis_number']) && isset($_POST['OWNER_CD'])){
   $chassisnumber=getChassisnumber($chskey);
   //$racknumber=getracknumber($rnumber);
   $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'slot_blade_tray_panel'";
          $autoquery=mysqli_query($con,$autoidquery);
           $keyassoc=mysqli_fetch_assoc($autoquery);
           $autoid=$keyassoc['AUTO_INCREMENT']; 
          $siteky=getHostkey($siteid);
          $okey=getOwnerkey($owner_cd);
          $mfrname=getmanufacturername($mfr);
          $modname=getModelnumber($mod);

  $sql="insert into slot_blade_tray_panel(OWNER_CD,chassis_number,slot_or_blade_number,manufacturer,model,port_count,created_on,created_by,site,rack,rack_key,chassis_key,`key`,owner_key,site_key,manufacturer_key,model_key,created_from,stage) values('$owner_cd','$chassisnumber','$bnumber','$mfrname','$modname','$port','$time','$uid','$siteid','$rnumber','$rkey','$chskey','$autoid','$okey','$siteky','$mfr','$mod','$createdfrm','$stage')";
  
    //mysqli_query($con,$sql);
///
$sqlc="select * from slot_blade_tray_panel where chassis_key='$chskey' and slot_or_blade_number='$bnumber' and record_state='L'";
	$uquery=mysqli_query($con,$sqlc);
	if(mysqli_num_rows($uquery)>0){
    $upfth=mysqli_fetch_assoc($uquery);
    $uprow=$upfth['id'];
    $upky=$upfth['key'];
    $upsql="update slot_blade_tray_panel set updated_on='$time',updated_by='$uid',updated_from='$createdfrm',record_state='H' where `id`='$uprow'";
    $upquery=mysqli_query($con,$upsql);
    $sql="insert into slot_blade_tray_panel(OWNER_CD,chassis_number,slot_or_blade_number,manufacturer,model,port_count,created_on,created_by,site,rack,rack_key,chassis_key,`key`,owner_key,site_key,manufacturer_key,model_key,created_from,stage) values('$owner_cd','$chassisnumber','$bnumber','$mfrname','$modname','$port','$time','$uid','$siteid','$rnumber','$rkey','$chskey','$upky','$okey','$siteky','$mfr','$mod','$createdfrm','$stage')";  
    if($upquery)
    $res=mysqli_query($con,$sql);
	}else{
    $res=mysqli_query($con,$sql);
  }
///
if($res){
  echo "1";
} else {
  echo "0";
}


   // echo json_encode($msg);
  }
	
?>
