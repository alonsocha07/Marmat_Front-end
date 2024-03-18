function getToken() {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Ldpr4IjAAAAAGkcsGhCZrp0OYvjMNKswiN7JCqf', { action: 'submit' }).then(function (token) {

            document.getElementById("LoginViewModelToken").value = token;
        });
    });
}


setInterval(getToken, 115000);


$(function () {
    getToken();
});