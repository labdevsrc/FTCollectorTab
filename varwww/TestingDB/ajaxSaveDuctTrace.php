  <?php

  session_start();
  include "conn.php"; 
  include "allfunctions.php";

    $sheath_mark1=$_POST['sheath_mark1'];
    $sheath_mark2=$_POST['sheath_mark2'];
    $sheath_mark3=$_POST['sheath_mark3'];
    $sheath_mark4=$_POST['sheath_mark4'];   

    $cable_id1=$_POST['cable_id1'];
    $cable_id2=$_POST['cable_id2'];
    $cable_id3=$_POST['cable_id3'];
    $cable_id4=$_POST['cable_id4'];

    $cable_id1_key=$_POST['cable_id1_key'];
    $cable_id2_key=$_POST['cable_id2_key'];
    $cable_id3_key=$_POST['cable_id3_key'];
    $cable_id4_key=$_POST['cable_id4_key']; 

    $cable_type1=$_POST['cable_type1'];
    $cable_type2=$_POST['cable_type2'];
    $cable_type3=$_POST['cable_type3'];
    $cable_type4=$_POST['cable_type4']; 

    $uid =  $_POST['uid']; 
    $time =  $_POST['time'];   
    $stage =  $_POST['stage'];   
    $ownerkey = $_POST['ownerkey'];
    $OWNER_CD =$_POST['OWNER_CD'];
    $job = $_POST['job'];
    $job_key = $_POST['job_key'];    
    $from_site = $_POST['from_site'];  
    $from_site_key = $_POST['from_site_key'];  
    $from_site_duct_key = $_POST['from_site_duct_key'];  
    $from_site_duct_direction = $_POST['from_site_duct_direction'];  
    $from_site_duct_direction_count = $_POST['from_site_duct_direction_count'];          
    $install_method = $_POST['install_method'];
    $uom =  $_POST['uom'];  

    $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'a_fiber_segment'";



    $key1 = $key2 = $key3 = $key4 = $locatepointkey = "0";
    
    if(!empty($cable_id1)){
        $autoquery=mysqli_query($con,$autoidquery); // execute query to get updated max id 
        $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT']; 

        $insertsql = "INSERT INTO a_fiber_segment(`key`, OWNER_CD, owner_key,job_key,job, cable_id,cable_id_key, from_site, from_site_key, from_site_duct_key, cable_type, uom,from_site_duct_direction, from_site_duct_direction_count, install_method, sheath_out, created_on, created_by) values($autoid,'$OWNER_CD','$ownerkey','$job_key','$job','$cable_id1','$cable_id1_key', '$from_site','$from_site_key','$from_site_duct_key','$cable_type1',  '$uom', '$from_site_duct_direction' ,'$from_site_duct_direction_count', '$install_method','$sheath_mark1', '$time','$uid')";
        $rescable1ins = mysqli_query($con, $insertsql);
        //$insertsql = "INSERT INTO a_fiber_segment(`key`,cable_id, sheath_out) values($autoid, '$cable_id1','$sheath_mark1')";
        //mysqli_query($con, $insertsql);
        $key1 = $autoid;
    }

    if(!empty($cable_id2)){
        $autoquery=mysqli_query($con,$autoidquery); // execute query to get updated max id 
        $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT']; 

        $insertsql = "INSERT INTO a_fiber_segment(`key`, OWNER_CD, owner_key,job_key,job, cable_id,cable_id_key, from_site, from_site_key, from_site_duct_key, cable_type,uom,from_site_duct_direction, from_site_duct_direction_count, install_method, sheath_out, created_on, created_by) values($autoid,'$OWNER_CD','$ownerkey','$job_key','$job','$cable_id2','$cable_id2_key', '$from_site','$from_site_key','$from_site_duct_key', '$cable_type2', '$uom', '$from_site_duct_direction' ,'$from_site_duct_direction_count', '$install_method','$sheath_mark2', '$time','$uid')";
        $rescable1ins = mysqli_query($con, $insertsql);     
        $key2 = $autoid;   
    }

    if(!empty($cable_id3)){

        $autoquery=mysqli_query($con,$autoidquery); // execute query to get updated max id 
        $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT']; 
        $insertsql = "INSERT INTO a_fiber_segment(`key`, OWNER_CD, owner_key,job_key,job, cable_id,cable_id_key, from_site, from_site_key, from_site_duct_key, cable_type,uom,from_site_duct_direction, from_site_duct_direction_count, install_method, sheath_out, created_on, created_by) values('$autoid','$OWNER_CD','$ownerkey','$job_key','$job','$cable_id3','$cable_id3_key', '$from_site','$from_site_key','$from_site_duct_key','$cable_type3', '$uom', '$from_site_duct_direction' ,'$from_site_duct_direction_count', '$install_method','$sheath_mark3', '$time','$uid')";
        $rescable1ins = mysqli_query($con, $insertsql);
        $key3 = $autoid;
    }        

    if(!empty($cable_id4)){

        $autoquery=mysqli_query($con,$autoidquery); // execute query to get updated max id 
        $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT']; 

        $insertsql = "INSERT INTO a_fiber_segment(`key`, OWNER_CD,owner_key,job_key,job, cable_id,cable_id_key, from_site, from_site_key, from_site_duct_key, cable_type,uom,from_site_duct_direction, from_site_duct_direction_count, install_method, sheath_out, created_on, created_by) values('$autoid','$OWNER_CD','$ownerkey','$job_key','$job','$cable_id4','$cable_id4_key', '$from_site','$from_site_key','$from_site_duct_key', '$cable_type4','$uom', '$from_site_duct_direction' ,'$from_site_duct_direction_count', '$install_method','$sheath_mark3', '$time','$uid')";
        $rescable1ins = mysqli_query($con, $insertsql);
        $key4 = $autoid;
    }     

    $getmax = "select max(locate_point_number) as maxnum from gps_point";
    $result = mysqli_query($con, $getmax);
    $r =mysqli_fetch_assoc($result);
    
    
    $data[]  = array();

    $data = array("key1"=>$key1,"key2"=>$key2,"key3"=>$key3,"key4"=>$key4, "locatepointkey"=> $r['maxnum']);
    echo(json_encode($data));