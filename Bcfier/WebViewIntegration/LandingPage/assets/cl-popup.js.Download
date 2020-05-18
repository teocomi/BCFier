/**
 * CodeLights: Modal Popup
 */
!function($){
	"use strict";
	var CLPopup = function(container){
		this.$container = $(container);

		this._events = {
			show: this.show.bind(this),
			afterShow: this.afterShow.bind(this),
			hide: this.hide.bind(this),
			preventHide: function(e){
				e.stopPropagation();
			},
			afterHide: this.afterHide.bind(this),
			resize: this.resize.bind(this),
			keypress: function(e){
				if (e.keyCode == 27) this.hide();
			}.bind(this)
		};

		// Event name for triggering CSS transition finish
		this.transitionEndEvent = (navigator.userAgent.search(/webkit/i)>0) ? 'webkitTransitionEnd' : 'transitionend';
		this.isFixed = !$cl.isMobile;

		this.$trigger = this.$container.find('.cl-popup-trigger');
		this.triggerType = this.$trigger.clMod('type');
		if (this.triggerType == 'load'){
			var delay = this.$trigger.data('delay') || 2;
			setTimeout(this.show.bind(this), delay * 1000);
		} else if (this.triggerType == 'selector') {
			var selector = this.$trigger.data('selector');
			if (selector) $cl.$body.on('click', selector, this._events.show);
		} else {
			this.$trigger.on('click', this._events.show);
		}
		this.$wrap = this.$container.find('.cl-popup-wrap')
			.clMod('pos', this.isFixed ? 'fixed' : 'absolute')
			.on('click', this._events.hide);
		this.$box = this.$container.find('.cl-popup-box');
		this.$overlay = this.$container.find('.cl-popup-overlay')
			.clMod('pos', this.isFixed ? 'fixed' : 'absolute')
			.on('click', this._events.hide);
		this.$container.find('.cl-popup-closer, .cl-popup-box-closer').on('click', this._events.hide);
		this.$box.on('click', this._events.preventHide);
		this.size = this.$box.clMod('size');

		this.timer = null;
	};
	CLPopup.prototype = {
		_hasScrollbar: function(){
			return document.documentElement.scrollHeight > document.documentElement.clientHeight;
		},
		_getScrollbarSize: function(){
			if ($cl.scrollbarSize === undefined) {
				var scrollDiv = document.createElement('div');
				scrollDiv.style.cssText = 'width: 99px; height: 99px; overflow: scroll; position: absolute; top: -9999px;';
				document.body.appendChild(scrollDiv);
				$cl.scrollbarSize = scrollDiv.offsetWidth - scrollDiv.clientWidth;
				document.body.removeChild(scrollDiv);
			}
			return $cl.scrollbarSize;
		},
		show: function(){
			clearTimeout(this.timer);
			this.$overlay.appendTo($cl.$body).show();
			this.$wrap.appendTo($cl.$body).show();
			if (this.size != 'f') {
				this.resize();
			}
			if (this.isFixed) {
				$cl.$html.addClass('cloverlay_fixed');
				// Storing the value for the whole popup visibility session
				this.windowHasScrollbar = this._hasScrollbar();
				if (this.windowHasScrollbar && this._getScrollbarSize()) {
					$cl.$html.css('margin-right', this._getScrollbarSize());
				}
			} else {
				this.$overlay.css({
					height: $cl.$document.height()
				});
				this.$wrap.css('top', $cl.$window.scrollTop());
			}
			$cl.$body.on('keypress', this._events.keypress);
			this.timer = setTimeout(this._events.afterShow, 25);
		},
		afterShow: function(){
			clearTimeout(this.timer);
			this.$overlay.addClass('active');
			this.$box.addClass('active');
			// UpSolution Themes Compatibility
			// TODO Move to themes
			if (window.$us !== undefined && $us.$canvas !== undefined) {
				$us.$canvas.trigger('contentChange');
			}
			$cl.$window.trigger('resize');
			if (this.size != 'f') {
				$cl.$window.on('resize', this._events.resize);
			}
		},
		hide: function(){
			clearTimeout(this.timer);
			if (this.size != 'f') {
				$cl.$window.off('resize', this._events.resize);
			}
			$cl.$body.off('keypress', this._events.keypress);
			this.$box.on(this.transitionEndEvent, this._events.afterHide);
			this.$overlay.removeClass('active');
			this.$box.removeClass('active');
			// Closing it anyway
			this.timer = setTimeout(this._events.afterHide, 1000);
		},
		afterHide: function(){
			clearTimeout(this.timer);
			this.$box.off(this.transitionEndEvent, this._events.afterHide);
			this.$overlay.appendTo(this.$container).hide();
			this.$wrap.appendTo(this.$container).hide();
			if (this.isFixed) {
				$cl.$html.removeClass('cloverlay_fixed');
				if (this.windowHasScrollbar) $cl.$html.css('margin-right', '');
				// To properly resize 3-rd party elements
				$cl.$window.trigger('resize');
			}
		},
		resize: function(){
			var animation = this.$box.clMod('animation'),
				padding = parseInt(this.$box.css('padding-top')),
				winHeight = $cl.$window.height(),
				popupHeight = this.$box.height();
			if (!this.isFixed) {
				this.$overlay.css('height', $cl.$document.height());
			}
			this.$box.css('top', Math.max(0, (winHeight - popupHeight) / 2 - padding));
		}
	};
	if (window.$cl === undefined) window.$cl = {};
	if ($cl.elements === undefined) $cl.elements = {};
	$cl.elements['cl-popup'] = CLPopup;
	if ($cl.maybeInit !== undefined) $cl.maybeInit();
}(jQuery);
