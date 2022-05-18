// https://blazor.syncfusion.com/documentation/appearance/themes?cs-save-lang=1&cs-lang=csharp

function setTheme(theme)
{
    document.getElementsByTagName('body')[0].style.display = 'none';

    let syncLink = document.getElementById('theme');
    syncLink.href = '_content/Radzen.Blazor/css/' + theme + '-base.css';

    setTimeout(function ()
    {
         document.getElementsByTagName('body')[0].style.display = 'block';
    }, 200);
}