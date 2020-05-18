/**
 * CodeLights: FlipBox
 */
!function($){
	"use strict";
	var CLFlipbox = function(container){
		// Common dom elements
		this.$container = $(container);
		this.$front = this.$container.find('.cl-flipbox-front');
		this.$frontH = this.$container.find('.cl-flipbox-front-h');
		this.$back = this.$container.find('.cl-flipbox-back');
		this.$backH = this.$container.find('.cl-flipbox-back-h');
		this.$btn = this.$container.find('.cl-btn');

		// Simplified animation for IE11
		if (!!window.MSInputMethodContext && !!document.documentMode){
			this.$container.clMod('animation', 'cardflip').find('.cl-flipbox-h').css({
				'transition-duration': '0s',
				'-webkit-transition-duration': '0s'
			});
		}

		// In chrome cube flip animation makes button not clickable. Replacing it with cube tilt
		var isWebkit = 'WebkitAppearance' in document.documentElement.style;
		if (isWebkit && this.$container.clMod('animation') === 'cubeflip' && this.$btn.length){
			this.$container.clMod('animation', 'cubetilt');
		}

		// For diagonal cube animations height should equal width (heometrical restriction)
		var animation = this.$container.clMod('animation'),
			direction = this.$container.clMod('direction');
		this.forceSquare = (animation == 'cubeflip' && ['ne', 'se', 'sw', 'nw'].indexOf(direction) != -1);

		// Container height is determined by the maximum content height
		this.autoSize = (this.$front[0].style.height == '' && !this.forceSquare);

		// Content is centered
		this.centerContent = (this.$container.clMod('valign') == 'center');

		if (this._events === undefined) this._events = {};
		$.extend(this._events, {
			resize: this.resize.bind(this)
		});
		if (this.centerContent || this.autoSize) {
			this.padding = parseInt(this.$front.css('padding-top'));
		}
		if (this.centerContent || this.forceSquare || this.autoSize) {
			$cl.$window.bind('resize load', this._events.resize);
			this.resize();
		}

		this.makeHoverable('.cl-btn');

		// Fixing css3 animations rendering glitch on page load
		setTimeout(function(){
			this.$back.css('display', '');
			this.resize();
		}.bind(this), 250);
	};
	CLFlipbox.prototype = {
		resize: function(){
			var width = this.$container.width(),
				height;
			if (this.autoSize || this.centerContent) {
				var frontContentHeight = this.$frontH.height(),
					backContentHeight = this.$backH.height();
			}
			// Changing the whole container height
			if (this.forceSquare || this.autoSize) {
				height = this.forceSquare ? width : (Math.max(frontContentHeight, backContentHeight) + 2 * this.padding);
				this.$front.css('height', height + 'px');
			} else {
				height = this.$container.height();
			}
			if (this.centerContent) {
				this.$front.css('padding-top', Math.max(this.padding, (height - frontContentHeight) / 2));
				this.$back.css('padding-top', Math.max(this.padding, (height - backContentHeight) / 2));
			}
		}
	};
	$.extend(CLFlipbox.prototype, $cl.mutators.Hoverable);
	if (window.$cl === undefined) window.$cl = {};
	if ($cl.elements === undefined) $cl.elements = {};
	$cl.elements['cl-flipbox'] = CLFlipbox;
	if ($cl.maybeInit !== undefined) $cl.maybeInit();
}(jQuery);
