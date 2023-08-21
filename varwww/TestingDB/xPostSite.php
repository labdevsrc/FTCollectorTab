 <?php 
    // session_start();
    include 'conn.php';
    ////////////variables//////////////////  
    $tag_number = $_POST['tag_number'];
    $stage = $_POST['stage'];
    $lattitude = $_POST['LATITUDE'];
    $longitude = $_POST['LONGITUDE'];

    // typecode
    $type = $_POST['type'];
    $typecode="0";
    if($type=="Building"){
    $typecode="1";
      }else if($type=="Cabinet"){
        $typecode="2";
     }else if($type=="Structure"){
        $typecode="3";
     }else if($type=="Pull Box"){
          $typecode="4";            
     }else if($type=="Electrical Site"){
           $typecode="5";
     }else if($type=="CCTV"){
           $typecode="18";
     }else if($type=="DMS"){
           $typecode="0";//42
     }else if($type=="WL"){
           $typecode="0"; // 26
     }else if($type=="RWIS"){
           $typecode="0"; //9
     }


    if(isset($_POST['offsetcallattitude']) && $_POST['offsetcallattitude']!="")
    {
      $lt=$_POST['offsetcallongitude'];
      $lat=$_POST['offsetcallattitude'];
      //
      $offlongi=$_SESSION['offsetlongitude'];
      $offlattti=$_SESSION['offsetlattitude'];

      $sql1="insert into Site (site_id,tag_number,type,plan_sheet,plan_sheet_item,LONGITUDE,LATITUDE,altitude,gps_accuracy,created_on,owner,created_by,job,stage,gps_time,OWNER_CD,`key`,OWNER_KEY,JOB_KEY,created_from,gps_offset_latitude,gps_offset_longitude,COUNTY) values('$tag_number','$tag_number','$typecode','$plansheet','$psitem','$lt','$lat','$altitude_m','$accuid','$time','$owner','$user','$jno','$stage','$gpstime','$ownerCD','$autoid','$ownerkey','$jobkey','$createdfrm','$offlattti','$offlongi','$usercounty')";
     
    }
    else
    {
        $sql1="insert into Site (site_id,tag_number,type,plan_sheet,plan_sheet_item,LONGITUDE,LATITUDE,altitude,gps_accuracy,created_on,owner,created_by,job,stage,gps_time,OWNER_CD,`key`,OWNER_KEY,JOB_KEY,created_from,COUNTY) values('$tag_number','$tag_number','$typecode','$plansheet','$psitem','$lt','$lat','$altitude_m','$accuid','$time','$owner','$user','$jno','$stage','$gpstime','$ownerCD','$autoid','$ownerkey','$jobkey','$createdfrm','$usercounty')";
    }
}