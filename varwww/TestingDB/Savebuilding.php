
	<?php
	
	include "conn.php";		

  //Come from post	
  
  $uid=$_POST['uid'];  //1
  $time=$_POST['time']; 
  $gpstime=$time;
  $accuracy=$_POST['accuracy'];
  $altitude=$_POST['altitude'];
  $owner1=$_POST['oid'];
  $ownerCD=$_POST['OWNER_CD'];
  $jno=$_POST['jobnum'];  //7

  $tag_number1=$_POST['tag']; 
  $tag_number1 = mysqli_real_escape_string($con,$tag_number1);
  $site_id1=$_POST['site2'];
  $site_id1 = mysqli_real_escape_string($con,$site_id1);
  $type1=$_POST['type2']; 
   $type1 = mysqli_real_escape_string($con,$type1);
  $SiteName1=$_POST['sitname2'];
  $SiteName1 = mysqli_real_escape_string($con,$SiteName1);
 
  $manufactured1=$_POST['manufactured_date'];
  $manufactured1 = mysqli_real_escape_string($con,$manufactured1);
  $manufacturer1=$_POST['manufacturer'];
  $manufacturer1 = mysqli_real_escape_string($con,$manufacturer1);
  $Model1=$_POST['mod2'];
  $Pictures1=$_POST['pic2']; ///
  $otag=$_POST['owner_tag_number'];
  $otag = mysqli_real_escape_string($con,$otag);
  $roadway=$_POST['roadway']; 
  $roadway = mysqli_real_escape_string($con,$roadway); 
  $pid=$_POST['pid']; 
  $pid = mysqli_real_escape_string($con,$pid); 
  $loct=$_POST['loct']; 
  $loct = mysqli_real_escape_string($con,$loct);
  $staddr=$_POST['staddr']; 
  $staddr = mysqli_real_escape_string($con,$staddr);
  $pscode=$_POST['pscode'];
   $pscode = mysqli_real_escape_string($con,$pscode);
  $btype=$_POST['btype'];  
  $btype = mysqli_real_escape_string($con,$btype);
  $orientation=$_POST['orientation'];
  $orientation = mysqli_real_escape_string($con,$orientation);

  $laneclosure=$_POST['laneclosure'];
  $laneclosure = mysqli_real_escape_string($con,$laneclosure);
  $dotdis=$_POST['dotdis'];
  $dotdis = mysqli_real_escape_string($con,$dotdis);
  $powr=$_POST['powr'];
  $powr = mysqli_real_escape_string($con,$powr);
  $elecsite=$_POST['elecsite'];
  $elecsite = mysqli_real_escape_string($con,$elecsite);
  $comm=$_POST['comm'];
  $comm = mysqli_real_escape_string($con,$comm);
  $commprovider=$_POST['commprovider'];
  $commprovider = mysqli_real_escape_string($con,$commprovider);
  $sitaddr=$_POST['sitaddr'];
  $sitaddr = mysqli_real_escape_string($con,$sitaddr);
  $udsowner=$_POST['udsowner']; 
  $udsowner = mysqli_real_escape_string($con,$udsowner);
  
  
  $record_count1='4';
  $record_state1=$_POST['rs2'];
   $record_state1 = mysqli_real_escape_string($con,$record_state1);
  $created_by1=$uid;
  $created_on1=$time;
  $height1=$_POST['height2'];
  $depth1=$_POST['depth2'];
  $width1=$_POST['width2'];
  $CLEAR_ZONE_IND1=$_POST['CLEAR_ZONE_IND2'];
 
      $intersection1=$_POST['intersect2'];
       $intersection1 = mysqli_real_escape_string($con,$intersection1);
      $material1=$_POST['material2'];
      $material1 = mysqli_real_escape_string($con,$material1);
      $mounting1=$_POST['mounting2'];
      $mounting1 = mysqli_real_escape_string($con,$mounting1);
      $filter_count1=$_POST['offilter2'];
      $filter_count1 = mysqli_real_escape_string($con,$filter_count1);
      $filter1=$_POST['fltrsize2'];
       $filter1 = mysqli_real_escape_string($con,$filter1);
      $sun_shield1=$_POST['sunshield2'];
      $sun_shield1 = mysqli_real_escape_string($con,$sun_shield1);
      $installed1=$_POST['installed2'];
      $installed1 = mysqli_real_escape_string($con,$installed1);
      $Description1=$_POST['comment2'];
      $Description1 = mysqli_real_escape_string($con,$Description1);

      $ELEC_DEV_ID1=$_POST['etc2'];
      $ELEC_DEV_ID1 = mysqli_real_escape_string($con,$ELEC_DEV_ID1);
      $fosc1=$_POST['fosc2'];
       $fosc1 = mysqli_real_escape_string($con,$fosc1);
      $vault1=$_POST['vault2'];
      $vault1 = mysqli_real_escape_string($con,$vault1);
      $DISTANCE_TO_EOTL1=$_POST['trlane2'];
      $DISTANCE_TO_EOTL1 = mysqli_real_escape_string($con,$DISTANCE_TO_EOTL1);
      $bucket_truck1=$_POST['bucket2'];
      $bucket_truck1 = mysqli_real_escape_string($con,$bucket_truck1);
      $serialno=$_POST['serialno'];
      $serialno = mysqli_real_escape_string($con,$serialno);
      $ground=$_POST['ground'];
       $ground = mysqli_real_escape_string($con,$ground);
      $key=$_POST['key'];
      $ktype=$_POST['ktype'];
      $traveldir=$_POST['traveldir'];
      $traveldir = mysqli_real_escape_string($con,$traveldir);
      $ownerkey=$_POST['owner_key'];
      $usercounty=$_POST['owner_county'];
      $jobkey=$_POST['jobkey'];
      $createdfrm="field collection";
	  
	
      ///////////////From main menu//////////////////
         if(isset($_POST['plansheet']) && $_POST['plansheet']!="")
            $plansheet=$_POST['plansheet'];
          else
            $plansheet="0";
          if(isset($_POST['psitem']) && $_POST['psitem']!="")
            $psitem=$_POST['psitem'];
          else
            $psitem="0";
          
          if(isset($_POST['stage'])) 
            $stage=$_POST['stage'];
          else
            $stage="0";
        
  //Come from post end , 31 values
  
  $msg= array("d" => "Sucessfully saved...","sts" =>"1");
  $msg1= array("d" => "Sucessfully updated...");

 

      $sql2="select * from Site where tag_number='$tag_number1'  and record_state='L'"; //and type='1'
      $resstmt=mysqli_query($con,$sql2);
      $row=mysqli_fetch_array($resstmt);
      $nopics=$row['Pictures'];
      $keyval=$row['key'];
      $rowid=$row['id'];
      //
     $LONGITUDE1=$row['LONGITUDE'];
     $LATITUDE1=$row['LATITUDE'];
     $offlongi=$row['gps_offset_longitude'];
     $offlattti=$row['gps_offset_latitude']; 



     $gravel_bottom=$row['gravel_bottom'];
     $lid_pieces=$row['lid_pieces']; 
     $has_apron=$row['has_apron']; 
     $rack_count=$row['rack_count']; 
          $manufacturer_key=$row['manufacturer_key']; 
     // disable - vicky
     //$altitude=$row['altitude'];
     //$accuracy=$row['gps_accuracy'];
     //$gpstime=$row['gps_time'];

 /*$sql="insert into Site (owner,OWNER_CD,tag_number,site_id,type,SiteName,manufactured,
    manufacturer,Model,Pictures,created_by,created_on,height,
    depth,width,CLEAR_ZONE_IND,LONGITUDE,LATITUDE) values('$owner1','$ownerCD','$tag_number1','$tag_number1','$type1','$SiteName1',
    '$manufactured1','$manufacturer1','$Model1','$Pictures1','$created_by1','$created_on1','$height1','$depth1','$width1','$CLEAR_ZONE_IND1',
    '$LONGITUDE1','$LATITUDE1')";*/   

   $sql="insert into Site (owner,OWNER_CD,tag_number,site_id,type,SiteName,manufactured,
    manufacturer,manufacturer_key, Model,Pictures,created_by,created_on,height,
    depth,width,CLEAR_ZONE_IND,LONGITUDE,LATITUDE,intersection,material,mounting,filter_count,filter,
    sun_shield,installed,Notes,ELEC_DEV_ID,fosc,vault, gravel_bottom, lid_pieces,has_apron,rack_count,manufacturer_key,
    DISTANCE_TO_EOTL,bucket_truck,job,serial_number,has_ground_rod,has_key,key_type,direction_of_travel,plan_sheet,plan_sheet_item,stage,gps_time,COUNTY,owner_tag_number,roadway,property_id,gps_accuracy,altitude,OWNER_KEY,JOB_KEY,location,street_address,postal_code,building_type,lane_closure_required,DOT_district,has_power_disconnect,electric_site_key,has_comms,comms_provider,site_street_address,UDS_owner,orientation,created_from,gps_offset_longitude,gps_offset_latitude) values('$owner1','$ownerCD','$tag_number1','$tag_number1','$type1','$SiteName1',
    '$manufactured1','$manufacturer1','$Model1','$Pictures1','$created_by1','$created_on1','$height1','$depth1','$width1','$CLEAR_ZONE_IND1',
    '$LONGITUDE1','$LATITUDE1','$intersection1','$material1','$mounting1','$filter_count1','$filter1',
    '$sun_shield1','$installed1','$Description1','$ELEC_DEV_ID1','$fosc1','$vault1','$gravel_bottom','$lid_pieces','$has_apron','$rack_count','$manufacturer_key',
    '$DISTANCE_TO_EOTL1','$bucket_truck1','$jno','$serialno','$ground','$key','$ktype','$traveldir','$plansheet','$psitem','$stage','$gpstime','$usercounty','$otag','$roadway','$pid','$accuracy','$altitude','$ownerkey','$jobkey','$loct','$staddr','$pscode','$btype','$laneclosure','$dotdis','$powr','$elecsite','$comm','$commprovider','$sitaddr','$udsowner','$orientation','$createdfrm','$offlongi','$offlattti')";
     /////////////////////////////// 
      $sql1="update Site set updated_by='$created_by1',updated_on= '$created_on1',record_state='H' where tag_number='$tag_number1'  and record_state='L'";  //and type='1' 
     //mysqli_query($con,$sql);
      
      if(mysqli_num_rows($resstmt)>0 && $uid!="")
      {              
        mysqli_query($con,$sql1);
        $sql="insert into Site (owner,OWNER_CD,tag_number,site_id,type,SiteName,manufactured,
        manufacturer,Model,Pictures,created_by,created_on,height,
        depth,width,CLEAR_ZONE_IND,LONGITUDE,LATITUDE,intersection,material,mounting,filter_count,filter,
        sun_shield,installed,Notes,ELEC_DEV_ID,fosc,vault, gravel_bottom, lid_pieces,has_apron,rack_count,manufacturer_key,
        DISTANCE_TO_EOTL,bucket_truck,job,serial_number,has_ground_rod,has_key,key_type,direction_of_travel,plan_sheet,plan_sheet_item,stage,gps_time,COUNTY,owner_tag_number,roadway,property_id,gps_accuracy,altitude,`key`,OWNER_KEY,JOB_KEY,location,street_address,postal_code,building_type,lane_closure_required,DOT_district,has_power_disconnect,electric_site_key,has_comms,comms_provider,site_street_address,UDS_owner,orientation,created_from,gps_offset_longitude,gps_offset_latitude) values('$owner1','$ownerCD','$tag_number1','$tag_number1','$type1','$SiteName1',
        '$manufactured1','$manufacturer1','$Model1','$Pictures1','$created_by1','$created_on1','$height1','$depth1','$width1','$CLEAR_ZONE_IND1',
        '$LONGITUDE1','$LATITUDE1','$intersection1','$material1','$mounting1','$filter_count1','$filter1',
        '$sun_shield1','$installed1','$Description1','$ELEC_DEV_ID1','$fosc1','$vault1','$gravel_bottom','$lid_pieces','$has_apron','$rack_count','$manufacturer_key',
        '$DISTANCE_TO_EOTL1','$bucket_truck1','$jno','$serialno','$ground','$key','$ktype','$traveldir','$plansheet','$psitem','$stage','$gpstime','$usercounty','$otag','$roadway','$pid','$accuracy','$altitude','$keyval','$ownerkey','$jobkey','$loct','$staddr','$pscode','$btype','$laneclosure','$dotdis','$powr','$elecsite','$comm','$commprovider','$sitaddr','$udsowner','$orientation','$createdfrm','$offlongi','$offlattti')";
    
        $ins=mysqli_query($con,$sql);         
        $sqlsiteid="select max(id) sid from Site";
        $resid=mysqli_query($con,$sqlsiteid);
        $geSiteid=mysqli_fetch_assoc($resid);      
        $site_id1=$geSiteid['sid'];
        ////////////////      
          
        $msg1= array("d" => "Sucessfully updated...","sts" =>"1");
        $msg2= array("d" => "Something went wrong.","sts" =>"0");
        if($ins){
           echo json_encode($msg1);
         } 
         else{
          $sql1="update Site set updated_on=NULL,updated_by=NULL,record_state='L' where tag_number='$tag_number1'  and record_state='H' and `id`='$rowid'";
                mysqli_query($con,$sql1);
                echo json_encode($msg2);  
          }
      }else{    
        $msg2= array("d" => $owner1." ".$ownerCD." ".$tag_number1."  ".$type1."   ".$SiteName1,"sts" =>"0");     
        //msg2 =  '$owner1'." ".'$ownerCD'." ".'$tag_number1'."  ".'$type1'."   ".'$SiteName1';
        //msg2 = "Test    ";
          echo json_encode($msg2);
      }
/////

  // uid,time,oid,OWNER_CD,jobnum,tag,site2,type2,sitname2,mfd2,mfr2,mod2,pic2,otag,roadway,pid,loct,staddr,pscode,btype,
  //orirntation,laneclosure,dotdis,powr,elecsite,comm,commprovider,sitaddr,udsowner,rs2,height2,depth2,width2,CLEAR_ZONE_IND2,
    //intersect2,material2,mounting2,offilter2,fltrsize2,sunshield2,installed2,comment2,etc2,fosc2,vault2,trlane2,bucket2,serialno,
  //ground,key,ktype,traveldir,owner_key,owner_county,jobkey,stage,plansheet,psitem

// uid:
// time:
// oid:
// OWNER_CD:
// jobnum:
// tag:
// site2:
// type2:
// sitname2:
// mfd2:
// mfr2:
// mod2:
// pic2:
// otag:
// roadway:
// pid:
// loct:
// staddr:
// pscode:
// btype:
// orirntation:
// laneclosure:
// dotdis:
// powr:
// elecsite:
// comm:
// commprovider:
// sitaddr:
// udsowner:
// rs2:
// height2:
// depth2:
// width2:
// CLEAR_ZONE_IND2:
// intersect2:
// material2:
// mounting2:
// offilter2:
// fltrsize2:
// sunshield2:
// installed2:
// comment2:
// etc2:
// fosc2:
// vault2:
// trlane2:
// bucket2:
// serialno:
// ground:
// key:
// ktype:
// traveldir:
// owner_key:
// owner_county:
// jobkey:
// stage:
// plansheet:
// psitem:
    	

           
?>