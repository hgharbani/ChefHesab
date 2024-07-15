// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function convertToJalaliDate(date) {
    if (date == null) return "";
    var persianDate = new Intl.DateTimeFormat('fa-IR').format(new Date(date));
    return PersianTools.digitsFaToEn(persianDate)
}




const accordionBtns = document.querySelectorAll(".accordion-button");
const accordionIcons = document.querySelectorAll(".accordion-icon");
const accordionContents = document.querySelectorAll(".accordion-content");
accordionBtns.forEach((btn, index) => {
    btn.addEventListener("click", () => {
        toggleAccordion(index, btn);
    });
});
function toggleAccordion(index, btn) {
    debugger;
    if ($(btn).parent().parent().children('.accordion-content').hasClass('show') == true) {
        $(btn).parent().parent().children('.accordion-content').removeClass('show')
        $(btn).parent().parent().children('.accordion-content').addClass('hidden')
    } else {
        $(btn).parent().parent().children('.accordion-content').removeClass('hidden')
        $(btn).parent().parent().children('.accordion-content').addClass('show')
    }

}

function ShowPopup1(windowid,url, title = '', width = '500px', height = 'auto', closeFunction = null) {
    var myWindow = $(windowid);

    myWindow.empty()
    $.ajax({
        url: url ,
        beforeSend: function (xhr) {

        }
    })
        .done(function (data) {
            myWindow.append(data)
            myWindow.kendoWindow({
                width: width,
                height: height,
                title: title,
                visible: false,
                actions: [
                   
                    "Close"
                ],
                close: closeFunction
            }).data("kendoWindow").center().open();
        });

}

function AddPopup(windowid) {
    var content = `
<div
  data-twe-modal-init
  class="fixed left-0 top-0 z-[1055] hidden h-full w-full overflow-y-auto overflow-x-hidden outline-none"
  id="${windowid}"
  tabindex="-1"
  aria-labelledby="exampleModalLabel"
  aria-hidden="true">
  <div
    data-twe-modal-dialog-ref
    class="pointer-events-none relative w-auto translate-y-[-50px] opacity-0 transition-all duration-300 ease-in-out min-[576px]:mx-auto min-[576px]:mt-7 min-[576px]:max-w-[500px]">
    <div
      class="pointer-events-auto relative flex w-full flex-col rounded-md border-none bg-white bg-clip-padding text-current shadow-4 outline-none dark:bg-surface-dark">
      <div
        class="flex flex-shrink-0 items-center justify-between rounded-t-md border-b-2 border-neutral-100 p-4 dark:border-white/10">
        <h5
          class="text-xl font-medium leading-normal text-surface dark:text-white"
          id="exampleModalLabel">
          ${Title}
        </h5>
        <button
          type="button"
          class="box-content rounded-none border-none text-neutral-500 hover:text-neutral-800 hover:no-underline focus:text-neutral-800 focus:opacity-100 focus:shadow-none focus:outline-none dark:text-neutral-400 dark:hover:text-neutral-300 dark:focus:text-neutral-300"
          data-twe-modal-dismiss
          aria-label="Close">
          <span class="[&>svg]:h-6 [&>svg]:w-6">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="currentColor"
              viewBox="0 0 24 24"
              stroke-width="1.5"
              stroke="currentColor">
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M6 18L18 6M6 6l12 12" />
            </svg>
          </span>
        </button>
      </div>

      <!-- Modal body -->
      <div class="relative flex-auto p-4" data-twe-modal-body-ref id="ModalConent">
        
      </div>

      <!-- Modal footer -->
      <div
        class="flex flex-shrink-0 flex-wrap items-center justify-end rounded-b-md border-t-2 border-neutral-100 p-4 dark:border-white/10">
        <button
          type="button"
          class="inline-block rounded bg-primary-100 px-6 pb-2 pt-2.5 text-xs font-medium uppercase leading-normal text-primary-700 transition duration-150 ease-in-out hover:bg-primary-accent-200 focus:bg-primary-accent-200 focus:outline-none focus:ring-0 active:bg-primary-accent-200 dark:bg-primary-300 dark:hover:bg-primary-400 dark:focus:bg-primary-400 dark:active:bg-primary-400"
          data-twe-modal-dismiss
          data-twe-ripple-init
          data-twe-ripple-color="light">
          Close
        </button>
        <button
          type="button"
          class="ms-1 inline-block rounded bg-primary px-6 pb-2 pt-2.5 text-xs font-medium uppercase leading-normal text-white shadow-primary-3 transition duration-150 ease-in-out hover:bg-primary-accent-300 hover:shadow-primary-2 focus:bg-primary-accent-300 focus:shadow-primary-2 focus:outline-none focus:ring-0 active:bg-primary-600 active:shadow-primary-2 dark:shadow-black/30 dark:hover:shadow-dark-strong dark:focus:shadow-dark-strong dark:active:shadow-dark-strong"
          data-twe-ripple-init
          data-twe-ripple-color="light">
          Save changes
        </button>
      </div>
    </div>
  </div>
</div>







`



}