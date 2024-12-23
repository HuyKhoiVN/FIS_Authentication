"use strict";

const accessKey = "Authorization";
const apiURL = "https://localhost:7174/api/v1/";

$("#loginForm").on("submit",function (e) {
    e.preventDefault();
    signIn();
});
//$(document).ready(function () {
//    localStorage.removeItem("token");
//})

async function signIn() {
    try {
        let data = {
            username: $("#username").val(),
            password: $("#password").val()
        };
        let errors = [];
        let swalSubTitle = "<p class='swal__admin__subtitle'>Đăng nhập không thành công!</p>";

        // Kiểm tra dữ liệu nhập
        if (data.username.trim().length === 0) {
            errors.push("Tài khoản không được để trống");
        }
        if (data.password.trim().length === 0) {
            errors.push("Mật khẩu không được để trống");
        }
        if (errors.length > 0) {
            let contentError = "<ul>";
            errors.forEach(function (item) {
                contentError += "<li class='text-start'>" + item + "</li>";
            });
            contentError += "</ul>";
            Swal.fire(
                'Đăng nhập' + swalSubTitle,
                contentError,
                'warning'
            );
            return;
        }

        // Gọi API bằng jQuery AJAX
        $.ajax({
            url: `${apiURL}Auth/loginService`,
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(data),
            success: function (result) {
                if (result.status == "200") {
                    let token = result.resources.accessToken;
                    let profile = result.resources.profile;

                    // Lưu token vào localStorage và cookie
                    localStorage.setItem("token", token);
                    document.cookie = `${accessKey}=${token}`;

                    localStorage.setItem("profile", profile);
                    sendToken();

                    // Hiển thị thông báo thành công
                    Swal.fire("Đăng nhập thành công", "Chào mừng <b>" + data.username + "</b> trở lại.", "success").then(function () {
                        location.href = "/";
                    });
                } else {
                    // Xử lý lỗi từ API
                    if (result.errors && result.errors.length > 1) {
                        let contentError = "<ul>";
                        result.errors.forEach(function (item) {
                            contentError += "<li class='text-start'>" + item + "</li>";
                        });
                        contentError += "</ul>";
                        Swal.fire(
                            'Đăng nhập' + swalSubTitle,
                            contentError,
                            'warning'
                        );
                    } else if (result.errors && result.errors.length === 1) {
                        Swal.fire(
                            "Đăng nhập",
                            result.errors[0],
                            "error"
                        );
                    } else {
                        Swal.fire(
                            "Đăng nhập",
                            "Đã xảy ra lỗi không xác định.",
                            "error"
                        );
                    }
                }
            },
            error: function (xhr, status, error) {
                Swal.fire(
                    "Đăng nhập",
                    "Đã có lỗi xảy ra xin vui lòng thử lại sau!",
                    "error"
                );
                console.error(error);
            }
        });
    } catch (e) {
        Swal.fire(
            "Đăng nhập",
            "Đã có lỗi xảy ra xin vui lòng thử lại sau!",
            "error"
        );
        console.error(e);
    }

}
function sendToken() {
    $.ajax({
        url: '/api/token/store', // API để lưu token vào trung gian
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            token: localStorage.getItem("token")   // Lấy token từ localStorage
        }),
        success: function () {
            console.log("Token stored successfully on the server.");
        },
        error: function (xhr) {
            console.error("Failed to store token:", xhr.responseText);
        }
    });

}

$("#btnLogin").on("click", function (e) {
    e.preventDefault();
    signIn();
})

$("#loginForm").on("input change keypress keydown", "input", function (e) {
    let text = $(this).val().trim();
    $(this).val(text);
    if (e.which == 13) {
        signIn();
    }
})
$(".none-space").on("change input blur", function () {
    let e = $(this);
    let text = e.val().trim();
    e.val(text);
})
$(".btn_show_pass").on("click", function (e) {
    var target = $($(this).attr("data-target"));
    if (target.attr("type") == "password") {
        target.attr("type", "text");
        $(this).html(`<i class="ki-duotone ki-eye-slash fs-3">
                                            <span class="path1 ki-uniEC07"></span>
                                            <span class="path2 ki-uniEC08"></span>
                                            <span class="path3 ki-uniEC09"></span>
                                            <span class="path4 ki-uniEC0A"></span>
                                        </i>`);
    }
    else {
        target.attr("type", "password");
        $(this).html(`<i class="ki-duotone ki-eye fs-3">
                                            <span class="path1 ki-uniEC0B"></span>
                                            <span class="path2 ki-uniEC0B"></span>
                                            <span class="path3 ki-uniEC0D"></span>
                                        </i>`);
    }
});