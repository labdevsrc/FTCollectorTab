<?php
    function getUsername($key){
        $sq="select * from end_user where `key`='$key' and record_state='L' ";
        $q=mysqli_query($GLOBALS['config'],$sq);
        $r=mysqli_fetch_assoc($q);
        $temp=$r['first_name']." ".$r['last_name'];
        return $temp;
    }




?>