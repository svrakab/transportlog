$(function () {
    var scheduler = $("#scheduler").dxScheduler({
        dataSource: Model.LoadList,
        views: ["day", "week", "timelineDay"],
        currentView: "day",
        currentDate: new Date(2015, 4, 25),
        firstDayOfWeek: 0,
        startDayHour: 9,
        endDayHour: 23,
        showAllDayPanel: false,
        height: 600,
        groups: ["theatreId"],
        crossScrollingEnabled: true,
        cellDuration: 20,
        editing: {
            allowAdding: false
        },
        resources: [{
                fieldExpr: "movieId",
                dataSource: moviesData,
                useColorAsDefault: true
            }, {
                fieldExpr: "theatreId",
                dataSource: theatreData
            }],
        appointmentTooltipTemplate: function (data, container) {
            var markup = getTooltipTemplate(getMovieById(data.movieId));
            markup.find(".edit").dxButton({
                text: "Edit details",
                type: "default",
                onClick: function () {
                    scheduler.showAppointmentPopup(data, false);
                }
            });
            return markup;
        },
        appointmentTemplate: function (data) {
            var movieInfo = getMovieById(data.movieId) || {};
            return $("<div class='showtime-preview'>" +
                "<div>" + movieInfo.text + "</div>" +
                "<div>Ticket Price: <strong>$" + data.price + "</strong>" +
                "</div>" +
                "<div>" + Globalize.formatDate(data.startDate, { time: "short" }) +
                " - " + Globalize.formatDate(data.endDate, { time: "short" }) +
                "</div>" +
                "</div>");
        },
        onAppointmentFormCreated: function (data) {
            var form = data.form, movieInfo = getMovieById(data.appointmentData.movieId) || {}, startDate = data.appointmentData.startDate;
            form.option("items", [{
                    label: {
                        text: "Movie"
                    },
                    editorType: "dxSelectBox",
                    dataField: "movieId",
                    editorOptions: {
                        items: moviesData,
                        displayExpr: "text",
                        valueExpr: "id",
                        onValueChanged: function (args) {
                            movieInfo = getMovieById(args.value);
                            form.getEditor("director")
                                .option("value", movieInfo.director);
                            form.getEditor("endDate")
                                .option("value", new Date(startDate.getTime() +
                                60 * 1000 * movieInfo.duration));
                        }
                    },
                }, {
                    label: {
                        text: "Director"
                    },
                    name: "director",
                    editorType: "dxTextBox",
                    editorOptions: {
                        value: movieInfo.director,
                        readOnly: true
                    }
                }, {
                    dataField: "startDate",
                    editorType: "dxDateBox",
                    editorOptions: {
                        width: "100%",
                        type: "datetime",
                        onValueChanged: function (args) {
                            startDate = args.value;
                            form.getEditor("endDate")
                                .option("value", new Date(startDate.getTime() +
                                60 * 1000 * movieInfo.duration));
                        }
                    }
                }, {
                    name: "endDate",
                    dataField: "endDate",
                    editorType: "dxDateBox",
                    editorOptions: {
                        width: "100%",
                        type: "datetime",
                        readOnly: true
                    }
                }, {
                    dataField: "price",
                    editorType: "dxRadioGroup",
                    editorOptions: {
                        dataSource: [5, 10, 15, 20],
                        itemTemplate: function (itemData) {
                            return "$" + itemData;
                        }
                    }
                }
            ]);
        }
    }).dxScheduler("instance");
    function getMovieById(id) {
        return DevExpress.data.query(moviesData)
            .filter("id", id)
            .toArray()[0];
    }
    function getTooltipTemplate(movieData) {
        return $("<div class='movie-tooltip'>" +
            "<img src='" + movieData.image + "' />" +
            "<div class='movie-info'>" +
            "<div class='movie-title'>" +
            movieData.text + " (" + movieData.year + ")" +
            "</div>" +
            "<div>" +
            "Director: " + movieData.director +
            "</div>" +
            "<div>" +
            "Duration: " + movieData.duration + " minutes" +
            "</div>" +
            "</div><br />" +
            "<div class='edit'></div>" +
            "</div>");
    }
});
