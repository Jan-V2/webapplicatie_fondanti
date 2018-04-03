function clear_fields(){
    $("#email").val("");
    $("#password").val("");
}
clear_fields();

$("document").ready(() => {

    function is_email(email) {
        let is_email_regex = /^\w+\@\w+\./;
        return is_email_regex.test(email);
    }

    function post_login(action, callback){
        let email = strip_spaces($("#email").val());
        let password = strip_spaces($("#password").val());
        if(password !== "" && email !== ""){
            if(is_email(email)){
                let data = {
                    action: action,
                    email: email,
                    password:password
                };
                $.post("/Admin/login",
                    data,
                    callback);
            }else{set_msg_and_clr("Het email adres is niet geldig."); }
        }else{set_msg_and_clr("Het email en het wachtwoord veld moeten bijde ingevuld zijn."); }
    }

    function set_msg_and_clr(string, do_clear=true) {
        console.log(string);
        $("#message_text").text(string);
        if( do_clear){clear_fields();}
    }

    function strip_spaces(str){return str.replace(/ /, "")}

    $("#button_login").click(() => {
        post_login("login",
            function (res, status) {
                console.log(res);
                if (res === "ok") {
                    window.location.href = "/admin/main";
                }else if (res ==="invalid_login"){
                    set_msg_and_clr("Ongeldige gebruikesnaam email combinatie.");
                }else {
                    set_msg_and_clr(res, false);
                }
            }
        );
    });

    $("#button_create").click(() => {
        if ($("#password").val() === $("#Wachtwoord_conf").val()){
            post_login("create_account",
                function (res, status) {
                    console.log(res);
                    if (res === "ok") {
                        window.location.href = "/admin/login?new=true";
                    }else{
                        set_msg_and_clr(res);
                    }
                }
            );
        }else{
            set_msg_and_clr("De twee wachtwoord velden zijn niet hezelfde.", false);
        }
    });
});