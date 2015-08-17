function checkIfBrowserIsMozilla() {
    var activeWindow = window,
        currentBrowser = activeWindow.navigator.appCodeName,
        isMozilla = currentBrowser === "Mozilla";

    if (isMozilla) {
        alert("Yes");
    } else {
        alert("No");
    }
}