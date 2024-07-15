"use strict";
(document.querySelector(".sweet-wrong").onclick = function () {
  Swal.fire("اوه...", "مشکلی پیش آمد !!", "error");
}),
  (document.querySelector(".sweet-message").onclick = function () {
    Swal.fire("سلام, اینجا یک پیام است !!");
  }),
  (document.querySelector(".sweet-text").onclick = function () {
    Swal.fire("سلام, در اینجا یک پیام وجود دارد!!", "زیباست ، اینطور نیست؟");
  }),
  (document.querySelector(".sweet-success").onclick = function () {
    Swal.fire("سلام, آفرین !!", "شما روی دکمه کلیک کردید!!", "success");
  }),
  (document.querySelector(".sweet-confirm").onclick = function () {
    Swal.fire(
      {
        title: "آیا مطمئناً حذف خواهید شد؟",
        text: "شما قادر به بازیابی این پرونده خیالی نخواهید بود !!",
        type: "warning",
        showCancelButton: !0,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "بله ، آن را حذف کنید !!",
        closeOnConfirm: !1,
      },
      function () {
        Swal.fire(
          "حذف شده !!",
          "سلام, پرونده خیالی شما حذف شده است !!",
          "success"
        );
      }
    );
  }),
  (document.querySelector(".sweet-success-cancel").onclick = function () {
    Swal.fire(
      {
        title: "آیا مطمئناً حذف خواهید شد؟",
        text: "شما قادر به بازیابی این پرونده خیالی نخواهید بود !!",
        type: "warning",
        showCancelButton: !0,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "بله ، آن را حذف کنید !!",
        cancelButtonText: "نه ، آن را لغو کنید !!",
        closeOnConfirm: !1,
        closeOnCancel: !1,
      },
      function (e) {
        e
          ? Swal.fire(
              "حذف شده !!",
              "سلام, پرونده خیالی شما حذف شده است !!",
              "success"
            )
          : Swal.fire(
              "لغو شد !!",
              "سلام, پرونده خیالی شما ایمن است !!",
              "error"
            );
      }
    );
  }),
  (document.querySelector(".sweet-image-message").onclick = function () {
    Swal.fire({
      title: "سوییت الرت !!",
      text: "سلام ، اینجا یک تصویر سفارشی است !!",
      imageUrl: "assets/images/hand.png",
      imageWidth: 80,
      imageHeight: 80,
      imageClass: "sweet-image",
    });
  }),
  (document.querySelector(".sweet-html").onclick = function () {
    Swal.fire({
      title: "سوییت الرت !!",
        text: "<span style='color:#ff0000'> سلام ، شما از اچ تی ام ال استفاده می کنید !!</span>",
      html: !0,
    });
  }),
  (document.querySelector(".sweet-auto").onclick = function () {
    Swal.fire({
      title: "هشدار نزدیک خودکار سوییت الرت !!",
      text: "سلام ، من در 2 ثانیه بسته خواهم شد !!",
      timer: 2e3,
      showConfirmButton: !1,
    });
  }),
  (document.querySelector(".sweet-prompt").onclick = function () {
    Swal.fire(
      {
        title: "ورودی را وارد کنید !!",
        text: "چیز جالبی بنویسید !!",
        type: "input",
        showCancelButton: !0,
        closeOnConfirm: !1,
        animation: "slide-from-top",
        inputPlaceholder: "چیزی بنویسید",
      },
      function (e) {
        return (
          !1 !== e &&
          ("" === e
            ? (Swal.fire.showInputError("شما باید چیزی بنویسید!"), !1)
            : void Swal.fire("سلام !!", "تو نوشتی: " + e, "success"))
        );
      }
    );
  }),
  (document.querySelector(".sweet-ajax").onclick = function () {
    Swal.fire(
      {
        title: "درخواست سوییت الرت آژاکس !!",
            text: "برای اجرای درخواست آژاکس ارسال کنید !!",
        type: "info",
        showCancelButton: !0,
        closeOnConfirm: !1,
        showLoaderOnConfirm: !0,
      },
      function () {
        setTimeout(function () {
          Swal.fire("سلام ، درخواست آژاکس به پایان رسید!!");
        }, 2e3);
      }
    );
  });
