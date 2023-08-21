  <?php

  session_start();
  include "conn.php"; 
  include "allfunctions.php";

    $uid =  $_POST['uid']; 
    $time =  $_POST['time'];   
    $stage =  $_POST['stage'];   
    $ownerkey = $_POST['ownerkey'];
    $OWNER_CD =$_POST['OWNER_CD'];
    $job = $_POST['job'];
    $job_key = $_POST['job_key'];    

    $locpoint_numstart =  $_POST['locpoint_numstart'];  
    $locpoint_numend =  $_POST['locpoint_numend'];  


    $from_site = $_POST['from_site'];  
    $from_site_key = $_POST['from_site_key']; 

    $tag_to = $_POST['tag_to'];  
    $tag_to_key = $_POST['tag_to_key'];     
    $duct_to = $_POST['duct_to'];  

  

    $key1 = $key2 = $key3 = $key4 = $locatepointkey = "0";
    
    $data[]  = array();


    $getmaxloc = "select max(locate_point_number) as maxloc, min(locate_point_number) as minloc from gps_point where tag_from ='$from_site'";
    $result = mysqli_query($con, $getmaxloc);


    if(mysqli_num_rows($result) > 0){
          $r = mysqli_fetch_assoc($result);
          $locstart = (int)$r['minloc'];
          $locend = (int)$r['maxloc'];

          $cntloc = 0;

        for($i = $locstart; $i <= $locend; $i++){
      		// update gps_point entry
      		$UpdateQuery="UPDATE gps_point SET tag_to='$tag_to',tag_to_key='$tag_to_key',
        		duct_to='$duct_to',
        		updated_on = '$time',
        		updated_by='$uid',
        		`time` = '$time' 
        		WHERE tag_from='$from_site' and locate_point_number = $i";
          mysqli_query($con, $UpdateQuery);
      		$cntloc++;
          }
          $data = array("key1"=>$locstart,"key2"=>$locend,"key3"=>$cntloc,"key4"=>$key4, "locatepointkey"=> $r['maxloc'], "status"=> "SUCCESS");
    }
    else
       $data = array("key1"=>"1","key2"=>"2","key3"=>"3","key4"=>"4", "locatepointkey"=> "5", "status"=> "NOT_FOUND_LOCPOINT");

    echo(json_encode($data));