function set_min_data() {
    let today = new Date();
    let dd = today.getDate() + 1;
    let mm = today.getMonth()+1; //January is 0!
    let yyyy = today.getFullYear();
    if(dd<10){
        dd='0'+dd
    }
    if(mm<10){
        mm='0'+mm
    }

    today = yyyy+'-'+mm+'-'+dd;
    document.getElementById("Date_inline").setAttribute("min", today);
}
set_min_data();
