(function ($) {
    $.ajax({
        url: "/signalr/hubs",
        dataType: "script",
        async: false
    });
}(jQuery));

$(function () {
    // Reference the auto-generated proxy for the hub.  
    var stream = $.connection.twitterStreamHub;
    // Create a function that the hub can call back to display messages.
    
    stream.client.streamTweets = function(message) {
        $("#hello").html(message);
    };
    
    // Get the user name and store it to prepend to messages.
    // Start the connection.
    $.connection.hub.start().done(function () { 
        stream.server.getTweets();
    });
});
// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}