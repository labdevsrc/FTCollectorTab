  <?php

  session_start();
  include "conn.php"; 
  include "allfunctions.php";

    $record_state='L';
    $owner_key=$_POST['owner_key'];
    $job=$_POST['job'];
    $job_key=$_POST['job_key'];   

    $OWNER_CD=$_POST['OWNER_CD'];
    $cable_id=$_POST['cable_id'];
    $cable_id_key= "9999";// $_POST['cable_id_key'];    
    $stage=$_POST['stage'];
    $cable_type=$_POST['cable_type'];

    $install_method=$_POST['install_method'];
    $from_site=$_POST['from_site'];
    $from_site_key=$_POST['from_site_key'];
    $from_site_duct=$_POST['from_site_duct']; 

    $from_site_duct_key=$_POST['from_site_duct_key'];
    $from_site_duct_direction=$_POST['from_site_duct_direction'];
    $from_site_duct_direction_count=$_POST['from_site_duct_direction_count'];
    $sheath_out=$_POST['sheath_out']; 

    $to_site =  ""; 
    $to_site_key =  "";   
    $to_site_duct =  "";   
    $to_site_duct_key = "";
    $to_site_duct_direction = "";
    $to_site_duct_direction_count = "";
    $sheath_in = $_POST['sheath_in'];    
    $cable_length = $_POST['cable_length'];  
    $geo_length = $_POST['geo_length'];  
    $uom = $_POST['uom'];  
    $created_on = $_POST['created_on'];
    $created_by = $_POST['created_by'];    
    $created_from = "field collector apk";

    $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'a_fiber_segment'";


    $key1 = $key2 = $key3 = $key4 = $locatepointkey = "0";
    
    if(!empty($cable_id)){
        $autoquery=mysqli_query($con,$autoidquery); // execute query to get updated max id 
        $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT']; 

        $insertsql = "INSERT INTO a_fiber_segment(`key`, OWNER_CD, owner_key,job_key,job, cable_id, cable_id_key, from_site, from_site_key, from_site_duct, from_site_duct_key, cable_type, uom,from_site_duct_direction, from_site_duct_direction_count, install_method, sheath_out, created_on, created_by, created_from) values($autoid,'$OWNER_CD','$owner_key','$job_key','$job','$cable_id','$cable_id_key', '$from_site','$from_site_key','$from_site_duct','$from_site_duct_key','$cable_type',  '$uom', '$from_site_duct_direction' ,'$from_site_duct_direction_count', '$install_method','$sheath_out','$created_on', '$created_by', '$created_from')";
        $rescable1ins = mysqli_query($con, $insertsql);
        if(mysqli_num_rows($rescable1ins) > 0)
        {
            $key1 = $autoid;
            $key2 = "DONE";
        }
        else{
             $key1 = "";
            $key2 = "FAIL";           
        }


    } 

    $getmax = "select max(locate_point_number) as maxnum from gps_point";
    $result = mysqli_query($con, $getmax);
    $r =mysqli_fetch_assoc($result);
    
    
    $data[]  = array();

    $data = array("key1"=>$key1,"key2"=>$key2,"key3"=>"","key4"=>"", "locatepointkey"=> $r['maxnum']);
    echo(json_encode($data));