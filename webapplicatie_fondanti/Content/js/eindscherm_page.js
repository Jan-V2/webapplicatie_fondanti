$("document").ready(() => {
    let elements = {
        type_cake: "taartsoort",
        bekleding:"bekleding",
        vulling:"vulling",
        niv_deco: "decoratieniveau"
    };

   function do_calc() {
       function grab_price(string) {
           let start_string = "(";
           let eind_string = "euro";
           let break_string = "Kies uw";
           if (string.indexOf(break_string) === -1){
               return parseFloat(
                   string.slice(string.indexOf(start_string) + start_string.length,
                       string.indexOf(eind_string))
               );
           }else{
               return 0;
           }
       }

       let eind_res = 0;
       let personen = + $("#personen_inline")[0].value;
       console.log(personen);

       for (let key in elements){
           let val = $("select#" +elements[key])[0];
           let txt = val[val.selectedIndex].textContent;
           let price = grab_price(txt);
           $("#calc_" + key).text(personen + " X " + price + " = ");
           let calced_price = personen * price;
           $("#res_" + key).text(calced_price);
           eind_res += calced_price;
       }
   }

   $("#test").click(() => {
       do_calc()
   });
});