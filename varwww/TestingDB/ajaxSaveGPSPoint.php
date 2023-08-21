  <?php

  session_start();
  include "conn.php"; 
  include "allfunctions.php";




    $longitude=$_POST['longitude']; 
    $lattitude=$_POST['lattitude'];
    $altitude=$_POST['altitude'];
    $accuracy=$_POST['accuracy'];

    $gps_offset_latitude=$_POST['gps_offset_latitude']; 
    $gps_offset_longitude=$_POST['gps_offset_longitude'];        
    $gps_offset_bearing=$_POST['gps_offset_bearing']; 
    $gps_offset_distance=$_POST['gps_offset_distance'];     

    $comment=$_POST['comment'];



    $uid =  $_POST['uid']; 
    $time =  $_POST['time'];   
    $jobnum = $_POST['jobnum'];
    $stage =  $_POST['stage'];   
    $ownerkey = $_POST['ownerkey'];
    $OWNER_CD = $_POST['OWNER_CD'];


    $startingkey =  $_POST['locate_point_number'];  
    $tag_from = $_POST['tag_from'];  
    $tag_from_key = $_POST['tag_from_key'];     
    $duct_from = $_POST['duct_from'];  
    $duct_from_key = $_POST['duct_from_key'];  

    $cable_id1 = $_POST['cable_id1'];
    $cable_type = $_POST['cable_type'];    
    $site_type =  $_POST['site_type'];  

    // get lattest id from gps_point table
    /*$getmax = "select max(locate_point_number) as maxnum from gps_point";
    $result = mysqli_query($con, $getmax);
    $r =mysqli_fetch_assoc($result);

    if($r['maxnum']  != $startingkey){
        $startingkey = $r['maxnum'];
    }*/

    if(!empty($gps_offset_bearing) && !empty($gps_offset_distance)){

        $insertsql = "INSERT INTO gps_point(locate_point_number, owner_key, OWNER_CD, job_number,
            tag_from ,tag_from_key,duct_from,
            gps_offset_latitude, gps_offset_longitude, gps_offset_bearing,gps_offset_distance,
            longitude,lattitude,altitude, accuracy, 
            cable_key, cable_type,`time`, created_on, created_by) 
        values('$startingkey','$ownerkey','$OWNER_CD','$jobnum', 
            '$tag_from', '$tag_from_key', '$duct_from',
        '$gps_offset_latitude', '$gps_offset_longitude','$gps_offset_bearing', '$gps_offset_distance', 
        '$longitude','$lattitude','$altitude','$accuracy',
        '$cable_id1', '$cable_type','$time','$time','$uid')";
        $rescable1ins = mysqli_query($con, $insertsql);
        $result2 = "INSERT_OFFSET_OK";

    }
    else {

        $insertsql = "INSERT INTO gps_point(locate_point_number, owner_key,  OWNER_CD,job_number,
            tag_from ,tag_from_key,duct_from,
            lattitude,longitude,altitude, accuracy,
            cable_key, cable_type,`time`, created_on, created_by) 
        values(
            '$startingkey','$ownerkey','$OWNER_CD','$jobnum', 
            '$tag_from', '$tag_from_key', '$duct_from',
        '$lattitude', '$longitude', '$altitude', '$accuracy',
        '$cable_id1', '$cable_type','$time','$time','$uid')";
        $rescable1ins = mysqli_query($con, $insertsql);
        $result2 = "INSERT_COORDS_OK";
    }

    $data[] = array("result"=> $startingkey, "result2"=>$result2, "RESULT3"=> $rescable1ins);
    echo(json_encode($data));