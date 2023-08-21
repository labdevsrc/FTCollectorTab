<?php
// This script invoked by SiteInputPage.xaml.cs client apk
// invoke function :  CloudDBService.PostCreateSiteAsync()
// input value : coords, coords with offset
// output table : Site

  include "conn.php";

        $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'Site'";
        $autoquery=mysqli_query($con,$autoidquery);
        $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT'];
        $user = $_POST['uid'];
        if($user!="" && $autoid!=""){
        $tag=$_POST['tag']; $typecode=$_POST['typecode']; $plansheet=$_POST['plansheet']; $psitem=$_POST['psitem'];
        $psitem=$_POST['psitem']; $psitem=$_POST['psitem']; 
        $altitude_m=$_POST['altitude']; $accuid=$_POST['accuracy']; $time=$_POST['time'];
        $owner=$_POST['owner']; $user=$_POST['user']; $jno=$_POST['jno']; $stage=$_POST['stage'];
        $gpstime=$_POST['gpstime']; $ownerCD=$_POST['ownerCD']; $ownerkey=$_POST['ownerkey'];
        $jobkey=$_POST['jobkey']; $createdfrm=$_POST['createdfrm']; $usercounty=$_POST['usercounty'];
        $lt=$_POST['longitude2']; 
        $lat=$_POST['lattitude2'];
        $gps_offset_latitude=$_POST['gps_offset_latitude']; 
        $gps_offset_longitude=$_POST['gps_offset_longitude'];        
        $gps_offset_bearing=$_POST['gps_offset_bearing']; 
        $gps_offset_distance=$_POST['gps_offset_distance'];          
        $qry=0;

        // create new Site
        $sql1="insert into Site (site_id,tag_number,type,plan_sheet,plan_sheet_item,LONGITUDE,LATITUDE,altitude,
        gps_offset_latitude, gps_offset_longitude, gps_offset_bearing, gps_offset_distance,
        gps_accuracy,created_on,owner,created_by,job,stage,gps_time,OWNER_CD,`key`,OWNER_KEY,JOB_KEY,created_from,COUNTY) 

        values('$tag','$tag','$typecode','$plansheet','$psitem','$lt','$lat','$altitude_m',
        '$gps_offset_latitude','$gps_offset_longitude','$gps_offset_bearing','$gps_offset_distance',
        '$accuid','$time','$owner','$user','$jno','$stage','$gpstime','$ownerCD','$autoid','$ownerkey','$jobkey','$createdfrm','$usercounty')";

        // verify tag_number already inserted to Site table
        $checktag="select * from Site where tag_number='$tag' and record_state='L'";
        $tagres=mysqli_query($con,$checktag);
        if(mysqli_num_rows($tagres)=="0"){
            $qry=mysqli_query($con,$sql1); 
        }
        if($qry){
            echo "CREATE_DONE";
            //echo "Status 1".$tag." ".$typecode." ".$lt." ".$lat." ".$altitude_m." ".$accuid;  // tag_number inserted successfully
        }else{
            echo "DUPLICATED";
            //echo "Status 0 , tagnumber ".$tag." ".$typecode." ".$lt." ".$lat." ".$altitude_m." ".$accuid; ;  // tag_number insert failed
        }   
  
} else{
    echo "0";  
}


  //tag,typecode,plansheet,psitem,longitude,lattitude,altitude,accuracy,time,owner,user,jno,stage,gpstime,ownerCD,autoid,ownerkey,jobkey,createdfrm,usercounty

//   tag:28122
// typecode:12
// plansheet:ps
// psitem:p-sit
// longitude:3.99
// lattitude:3.88
// altitude:-8.00
// accuracy:9.00
// time:2022-01-28 10:30:51
// owner:23
// user:22
// jno:1234
// stage:A
// gpstime:2022-01-28 10:30:51
// ownerCD:OW
// ownerkey:23
// jobkey:22
// createdfrm:field
// usercounty:test

?>
