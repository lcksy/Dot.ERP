/* Set the defaults for DataTables initialisation */
$.extend( true, $.fn.dataTable.defaults, {
	"sDom": "<'row-fluid'<'span6'l><'span6'f>r>t<'row-fluid'<'span6'i><'span6'p>>",
	"sPaginationType": "bootstrap",
	"oLanguage": {
		"sLengthMenu": "_MENU_ records per page"
	}
} );


/* Default class modification */
$.extend( $.fn.dataTableExt.oStdClasses, {
	"sWrapper": "dataTables_wrapper form-inline"
} );


/* API method to get paging information */
$.fn.dataTableExt.oApi.fnPagingInfo = function ( oSettings )
{
	return {
		"iStart":         oSettings._iDisplayStart,
		"iEnd":           oSettings.fnDisplayEnd(),
		"iLength":        oSettings._iDisplayLength,
		"iTotal":         oSettings.fnRecordsTotal(),
		"iFilteredTotal": oSettings.fnRecordsDisplay(),
		"iPage":          Math.ceil( oSettings._iDisplayStart / oSettings._iDisplayLength ),
		"iTotalPages":    Math.ceil( oSettings.fnRecordsDisplay() / oSettings._iDisplayLength )
	};
};


/* Bootstrap style pagination control */
$.extend( $.fn.dataTableExt.oPagination, {
	"bootstrap": {
		"fnInit": function( oSettings, nPaging, fnDraw ) {
			var oLang = oSettings.oLanguage.oPaginate;
			var fnClickHandler = function ( e ) {
				e.preventDefault();
				if ( oSettings.oApi._fnPageChange(oSettings, e.data.action) ) {
					fnDraw( oSettings );
				}
			};

			$(nPaging).addClass('pagination').append(
				'<ul>' +
                    '<li class="first disabled"><a href="#"><span class="icon-step-backward"></span> <span class="hidden-480"></span></a></li>' +
					'<li class="prev disabled"><a href="#"><span class="icon-backward"></span> <span class="hidden-480"></span></a></li>' +
                    '<li class="active"></li>' +
					'<li class="next disabled"><a href="#"><span class="hidden-480"></span> <span class="icon-forward"></span></a></li>' +
                    '<li class="last disabled"><a href="#"><span class="hidden-480"></span> <span class="icon-step-forward"></span></a></li>' +
				'</ul>'
			);
			var els = $('a', nPaging);
			$(els[0]).bind('click.DT', { action: "first" }, fnClickHandler);
			$(els[1]).bind('click.DT', { action: "previous" }, fnClickHandler );
			$(els[2]).bind('click.DT', { action: "next" }, fnClickHandler);
			$(els[3]).bind('click.DT', { action: "last" }, fnClickHandler);
		},

		"fnUpdate": function (oSettings, fnDraw) {
		    var iListLength = 5;
		    var oPaging = oSettings.oInstance.fnPagingInfo();
			var an = oSettings.aanFeatures.p;

			if (oPaging.iPage <= 0)
			    oPaging.iPage = 0;
			else if (oPaging.iPage > oPaging.iTotalPages)
			    oPaging.iPage = oPaging.iTotalPages;

			for ( i=0, iLen=an.length ; i<iLen ; i++ ) {
				// Remove the middle elements
			    $('li.active input', an[i]).remove();

				// Add the new list items and their event handlers
			    $('<input style="float:left;border:1px solid #ddd;border-left-style:none;text-align:center;width:30px;height:20px;" type="text" value="' + (oPaging.iPage + 1) + '"/>')
                    .appendTo($('li.active', an[i])[0])
                    .bind('blur', function (e) {
                        e.preventDefault();
                        var currpage = parseInt(e.currentTarget.value, 10);

                        if (currpage <= 0 || isNaN(currpage)) 
                            currpage = oPaging.iPage + 1;
                        else if (currpage > oPaging.iTotalPages) 
                            currpage = oPaging.iTotalPages;

                        oSettings._iDisplayStart = (currpage - 1) * oPaging.iLength;
                        fnDraw(oSettings);
                    });

				// Add / remove disabled classes from the static elements
				if ( oPaging.iPage === 0 ) {
				    $('li.first', an[i]).addClass('disabled');
				    $('li.prev', an[i]).addClass('disabled');
				} else {
				    $('li:first', an[i]).removeClass('disabled');
				    $('li.prev', an[i]).removeClass('disabled');
				}

				if ( oPaging.iPage === oPaging.iTotalPages-1 || oPaging.iTotalPages === 0 ) {
				    $('li.next', an[i]).addClass('disabled');
				    $('li.last', an[i]).addClass('disabled');
				} else {
				    $('li.next', an[i]).removeClass('disabled');
				    $('li.last', an[i]).removeClass('disabled');
				}
			}
		}
	}
} );


/*
 * TableTools Bootstrap compatibility
 * Required TableTools 2.1+
 */
if ( $.fn.DataTable.TableTools ) {
	// Set the classes that TableTools uses to something suitable for Bootstrap
	$.extend( true, $.fn.DataTable.TableTools.classes, {
		"container": "DTTT btn-group",
		"buttons": {
			"normal": "btn",
			"disabled": "disabled"
		},
		"collection": {
			"container": "DTTT_dropdown dropdown-menu",
			"buttons": {
				"normal": "",
				"disabled": "disabled"
			}
		},
		"print": {
			"info": "DTTT_print_info modal"
		},
		"select": {
			"row": "active"
		}
	} );

	// Have the collection use a bootstrap compatible dropdown
	$.extend( true, $.fn.DataTable.TableTools.DEFAULTS.oTags, {
		"collection": {
			"container": "ul",
			"button": "li",
			"liner": "a"
		}
	} );
}