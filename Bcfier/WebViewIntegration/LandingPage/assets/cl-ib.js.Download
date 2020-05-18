/**
 * CodeLights: Interactive banner
 */
!function($){
	"use strict";
	var CLIb = function(container){
		// Common dom elements
		this.$container = $(container);
		this.makeHoverable();
	};
	$.extend(CLIb.prototype, $cl.mutators.Hoverable);
	if (window.$cl === undefined) window.$cl = {};
	if ($cl.elements === undefined) $cl.elements = {};
	$cl.elements['cl-ib'] = CLIb;
	if ($cl.maybeInit !== undefined) $cl.maybeInit();
}(jQuery);
