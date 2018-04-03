$("document").ready(() => {

    let table = $("#admin_tabel");

    function trim_end_spaces(string) {
        let spaces = 0;
        for (let i in _.range(string.length)){
            let char = string[(string.length  - 1)- i];
            if (char === " "){
                spaces++;
            }else{
                break
            }
        }
        if (spaces === 0){
            return string
        }else{
            return string.slice(0, - spaces);
        }
    }



    table.click((e) => {
        function get_col_getter(row){return (col_num) => {return trim_end_spaces(row.cells[col_num].textContent)}}

        let id = e.originalEvent.target.id;
        let update_prijs_id = "update_prijs_";
        let verwijder_id = "verwijder_";
        let verander_beschrijving_id ="beschrijving_";

        if (id.indexOf(update_prijs_id) === 0) {
            let i = + id.replace(update_prijs_id, "");
            let nieuwe_prijs_input = $("#nieuwe_prijs_"+i)[0];
            let nieuwe_prijs = nieuwe_prijs_input.value;
            console.log(nieuwe_prijs);
            if (nieuwe_prijs !== 0 && nieuwe_prijs !== ""){
                let row = $("tr#row_"+i)[0];
                let get_col_val = get_col_getter(row);
                let type = get_col_val(0);
                console.log(row.cells[0].textContent);
                let naam = get_col_val(1);

                let data = {
                    action: "update_prijs",
                    type:type,
                    naam:naam,
                    nieuwe_prijs: nieuwe_prijs
                };
                console.log(data);
                if (confirm("Weet je zeker dat je prijs van "+naam+" wilt veranderen naar "+nieuwe_prijs+" cent?")) {
                    $.post("/Admin/main",
                        data,
                        function (res, status) {
                            if (res === "ok") {
                                console.log("got ok");
                                nieuwe_prijs_input.value = "";
                                row.cells[2].textContent = nieuwe_prijs
                            } else {
                                alert(res)
                            }
                        });
                }
            }
        }


        if (id.indexOf(verwijder_id) === 0) {
            let i = + id.replace(verwijder_id, "");
            let row = $("tr#row_"+i)[0];
            let get_col_val = get_col_getter(row);
            let type = get_col_val(0);
            let naam = get_col_val(1);

            if (confirm("Weet je zeker dat je "+naam+" wilt verwijderen?")) {
                $.post("/Admin/main",
                    {
                        action: "delete_item",
                        type:type,
                        naam:naam,
                    },
                    function (res, status) {
                        if (res === "ok") {
                            row.parentNode.removeChild(row);
                        } else {
                            alert(res)
                        }
                    });
            }

        }

        /*    if (id.indexOf(verander_beschrijving_id) === 0) {
                if (confirm("Are you sure you would like to delete this item.")) {
                    let i = + id.replace(verander_beschrijving_id, "");
                    item = $("li#wl_item_" + i).text();
                    console.log(item);
                    $.post("/Admin_Page",
                        {
                            action: "delete_item",
                            item: item
                        },
                        function (data, status) {
                            if (data === "ok") {

                            } else {
                                alert(data)
                            }
                        });
                }
            }*/
    });

    $("#insert_onderdeel").click(() => {
        let data_row = $("#insert_onderdeel_table")[0];
        let get_val_from_cell = (i) => {return data_row.cells[i].childNodes[1].value};

        let type = get_val_from_cell(0);
        let naam = get_val_from_cell(1);
        let prijs = get_val_from_cell(2);
        if(naam !== "" && prijs !== ""){
            if (confirm("Weet u zeker dat u een "+type+" onderdeel wil aanmaken, met "+naam+" als de naam, en "+prijs+" als de prijs in cent?")){
                $.post("/Admin/main",
                    {
                        action: "insert_item",
                        type:type,
                        naam:naam,
                        prijs:prijs
                    },
                    function (res, status) {
                        if (res === "ok") {
                            res = "Onderdeel aangemaakt."
                        }
                        alert(res)

                    });
            }
        }else{alert("Niet alle velden ingevuld.")}
    });


    $("#reset_db").click((e) => {
        if (confirm("Weet u zeker dat u de database wilt resetten?")){
            $.post("/Admin/main",
                {
                    action: "reset_db",
                },
                function (res, status) {
                    if (res === "ok") {
                        location.reload(true);
                    }
                    alert(res)
                });
        }

    });
});

