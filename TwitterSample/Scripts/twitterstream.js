(function ($) {
    $.ajax({
        url: "/signalr/hubs",
        dataType: "script",
        async: false
    });
}(jQuery));

// A simple templating method for replacing placeholders enclosed in curly braces.
if (!String.prototype.supplant) {
    String.prototype.supplant = function (o) {
        return this.replace(/{([^{}]*)}/g,
            function (a, b) {
                var r = o[b];
                return typeof r === 'string' || typeof r === 'number' ? r : a;
            }
        );
    };
}

$(function() {
    // Reference the auto-generated proxy for the hub.  
    var stream = $.connection.twitterStreamHub;
    // Create a function that the hub can call back to display messages.
    var rowTemplate = '<div class="span4"> ' +
        '<h2>{TwitterAccount}</h2> ' +
        '<br/> ' +
        '<h3>Tweets: <span class="label">{NumberOfTweets}</span></h3>' +
        '<h3>Mentions Of Others: <span class="label">{NumberOfMentionsOfOthers}</span></h3>' +
        '<div class="list-group">' +
        '</div>' +
        '</span4>';
    
    var tweetTemplate = '<a href="#" class="list-group-item">' +
        '<h4 class="list-group-item-heading"><span class="label label-info">{TweetTime}</span></h4>' +
        '<p class="list-group-item-text">{Tweet}</p>' +
        '</a>';
    
    stream.client.streamTweets = function(message) {
        $("#hello").html(message);
    };

    // Get the user name and store it to prepend to messages.
    // Start the connection.
    $.connection.hub.start().done(function() {
        stream.server.getTweets(["@pay_by_phone", "@PayByPhone", "@PayByPhone_UK"]).done(function(tweetStreams) {
            $.each(tweetStreams, function () {
                var $row = $(".row").append(rowTemplate.supplant(this));
                $.each(this.Contents, function() {
                    $(".list-group", $row).append(tweetTemplate.supplant(this));
                });
                
                //alert(this.TwitterAccount + ' ' + this.NumberOfTweets);
            });
        });
    });
});
