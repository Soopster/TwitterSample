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
    var rowTemplate = '<div class="col-lg-4"> ' +
        '<div class ="panel tweetStreams">' +
        '<div class="panel panel-info">'+
        '<div class="panel-heading">'+
        '<h3>{TwitterAccount}</h3> ' +
        '</div>'+
        '<h4>Tweets: <span class="label">{NumberOfTweets}</span></h4>' +
        '<h4>Mentions Of Others: <span class="label">{NumberOfMentionsOfOthers}</span></h4>' +
        '</div>'+
        '<div class="list-group">' +
        '</div>' +
        '</div>' +
        '</div>';
    
    var tweetTemplate = '<a href="#" class="list-group-item">' +
        '<h4 class="list-group-item-heading"><span class="label label-info">{TweetTime}</span></h4>' +
        '<p class="list-group-item-text">{Tweet}</p>' +
        '</a>';

    var fillTemplate = function(tweetStreams) {
        $.each(tweetStreams, function() {
            var $row = $(".row").append(rowTemplate.supplant(this));
            $.each(this.Contents, function() {
                $(tweetTemplate.supplant(this)).hide().appendTo(".list-group:last", $row).fadeIn(1000);
            });
        });
    };

    stream.client.updateStream = function(tweetStreams) {
        $(".row").empty();

        fillTemplate(tweetStreams);
    };

    // Get the user name and store it to prepend to messages.
    // Start the connection.
    $.connection.hub.start().done(function () {
        
        stream.server.getTweets(["@pay_by_phone", "@PayByPhone", "@PayByPhone_UK"]).done(function(tweetStreams) {
            $("#loading").remove();
            
            fillTemplate(tweetStreams);
        });
    });
});
