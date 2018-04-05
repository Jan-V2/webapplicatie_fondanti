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
               let numslice = string.slice(string.lastIndexOf(start_string) + start_string.length,
                   string.lastIndexOf(eind_string));
               return parseFloat(numslice);
           }else{
               return 0;
           }
       }

       let eind_res = 0;
       let personen = + $("#personen_inline")[0].value;

       for (let key in elements){
           let val = $("#" +elements[key])[0];
           let txt = val[val.selectedIndex].textContent;
           let price = grab_price(txt);
           $("#calc_" + key).text(personen + " X " + price + " = ");
           let calced_price = personen * price;
           $("#res_" + key).text( "€ " + calced_price);
           eind_res += calced_price;
       }
       $("#totaal_prijs").text("€ " +  eind_res);
   }

   $("#test").click(() => {
       do_calc()
   });


   $("#main_form").change((event) => {
       do_calc();
   });

   do_calc();
});