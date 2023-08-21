<?php
    function gettagnumber($key){
        $sq="select * from Site where `key`='$key' and record_state='L' ";
        $q=mysqli_query($GLOBALS['config'],$sq);
        $r=mysqli_fetch_assoc($q);
        $temp=$r['tag_number'];
        return $temp;
    }
    function getcableid($key){
        $sq="select * from a_fiber_cable where `key`='$key' and record_state='L' ";
        $q=mysqli_query($GLOBALS['config'],$sq);
        $r=mysqli_fetch_assoc($q);
        $temp=$r['cable_id'];
        return $temp;
    }
    function getcabletype($key){
      $sq="select * from a_fiber_cable where `key`='$key' and record_state='L' ";
      $q=mysqli_query($GLOBALS['config'],$sq);
      $r=mysqli_fetch_assoc($q);
      $temp=$r['cable_type'];
      return $temp;
  }
    function getracknumber($key){
        $sq="select * from rack_rail_shelf where `key`='$key' and record_state='L' ";
        $q=mysqli_query($GLOBALS['config'],$sq);
        $r=mysqli_fetch_assoc($q);
        $temp=$r['number'];
        return $temp;
    }
      function getrackkey($num,$sitid){
        $sq="select * from rack_rail_shelf where `number`='$num' and record_state='L' and site_id='$sitid'";
        $q=mysqli_query($GLOBALS['config'],$sq);
        $r=mysqli_fetch_assoc($q);
        $temp=$r['key'];
        return $temp;
    }
    function getHostkey($tag){
        $sq="select * from Site where `tag_number`='$tag' and record_state='L' ";
        $q=mysqli_query($GLOBALS['config'],$sq);
        $r=mysqli_fetch_assoc($q);
        $temp=$r['key'];
        return $temp;
    }
    
    function ff(){
        //return "th";
        return "tessss";
    }
    function getOwnerkey($ownerCD){
          //require_once "conn.php" ;   
          $ownerkeysql="select * from `owner` where OWNER_CD='$ownerCD' and record_state='L'";
          $resoquery=mysqli_query($GLOBALS['config'],$ownerkeysql);
          $resokey=mysqli_fetch_assoc($resoquery);
          $ownerkeys=$resokey['key'];
          return $ownerkeys;
    }
    function getChassisnumber($key){
          //require_once "conn.php" ;   
          $sql="select * from chassis where `key`='$key' and record_state='L'";
          $qry=mysqli_query($GLOBALS['config'],$sql);
          $res=mysqli_fetch_assoc($qry);
          $keys=$res['number'];
          return $keys;
    }

      function getModelnumber($key){
          //require_once "conn.php" ;   
          $sql="select * from model where `key`='$key' and record_state='L'";
          $qry=mysqli_query($GLOBALS['config'],$sql);
          $res=mysqli_fetch_assoc($qry);
          $keys=$res['number'];
          return $keys;
    }
        function getColor($hex){
           $sql="select * from code_colors where color_hex='$hex' and record_state='L'";
          $qry=mysqli_query($GLOBALS['config'],$sql);
          $res=mysqli_fetch_assoc($qry);
          $col=$res['color'];
          return $col;
    }
    function getPagefieldval($table,$keycol,$userid){
       $sql="select * from code_help_text where `language`=(select `language` from end_user where `key`='$userid' and record_state='L') and `table`='$table' and `key`='$keycol' and record_state='L';";
          $qry=mysqli_query($GLOBALS['config'],$sql);
          $res=mysqli_fetch_assoc($qry);
          
          return $res;
    }
     function getModelslot($modkey){
       $sql="select * from model where record_state='L' and `key`='$modkey'";
          $qry=mysqli_query($GLOBALS['config'],$sql);
          $res=mysqli_fetch_assoc($qry);           
          return $res['max_slots'];
    }
    //manu
    function getmanufacturername($mfrkey){
       $sql="select * from manufacturer where record_state='L' and `key`='$mfrkey'";
          $qry=mysqli_query($GLOBALS['config'],$sql);
          $res=mysqli_fetch_assoc($qry);           
          return $res['name'];
    }
    function getModdesc($mod){
       $sql="select * from model where record_state='L' and `key`='$mod'";
          $qry=mysqli_query($GLOBALS['config'],$sql);
          $res=mysqli_fetch_assoc($qry);           
          return $res['MODEL_DESC'];
    }
    //
    function getductdirection($ky){
      $sql="select * from CONDUITS_GROUP where record_state='L' and `key`='$ky'";
         $qry=mysqli_query($GLOBALS['config'],$sql);
         $res=mysqli_fetch_assoc($qry);           
         return $res['direction'];
   }
    //
    function getductdirection_count($ky){
      $sql="select * from CONDUITS_GROUP where record_state='L' and `key`='$ky'";
         $qry=mysqli_query($GLOBALS['config'],$sql);
         $res=mysqli_fetch_assoc($qry);           
         return $res['direction_count'];
   }

    function checkduct(){       
          $recordstate="L";
          $ownerCD=$_SESSION['OWNER_CD'];  
          $tag=$_SESSION['tag'];
          $sql="select * from CONDUITS_GROUP where record_state='$recordstate' and OWNER_CD='$ownerCD' and host_tag_number='$tag'";
          $query=mysqli_query($GLOBALS['config'],$sql);
          if(mysqli_num_rows($query)>0){
            return "1";
          } else {
            return "0";
          }
    }
  //******** */
  function lastusedcable(){       
    $recordstate="L";
    $uid=$_SESSION['uid']; 
    $jno=$_SESSION['jobnum'];
    //$sql="select * from gps_point where job_number='$jno' and created_by='$uid' order by created_on desc limit 1";
    $sql="select * from sheath_mark where created_by='$uid' order by created_on desc limit 1";
    $query=mysqli_query($GLOBALS['config'],$sql);
    $cabq=mysqli_fetch_assoc($query);
    if(mysqli_num_rows($query)>0){
      return $cabq['cable_key'];
    } else {
      return "0";
    }
}
//
function lastusedsite(){       
  $recordstate="L";
  $uid=$_SESSION['uid']; 
  $jno=$_SESSION['jobnum'];
  $sql="select * from Site where record_state='L' and created_by='$uid' order by created_on desc limit 1";
  $query=mysqli_query($GLOBALS['config'],$sql);
  $siteqr=mysqli_fetch_assoc($query);
  if(mysqli_num_rows($query)>0){
    return $siteqr['key'];
  } else {
    return "0";
  }
}



?>