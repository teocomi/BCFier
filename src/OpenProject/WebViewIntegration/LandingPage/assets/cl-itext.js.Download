/**
 * CodeLights: Interactive Text
 */
!function($){
	"use strict";
	var CLItext = function(container){
		this.$container = $(container);
		var data = this.$container[0].onclick() || {};
		this.$container.removeAttr('onclick');
		this.type = this.$container.clMod('type');
		this.animateChars = (this.type.substring(this.type.length - 5) == 'Chars');
		if (this.animateChars) {
			this.type = this.type.substring(0, this.type.length - 5);
		}
		this.duration = parseInt(data.duration) || 1000;
		this.delay = parseInt(data.delay) || 5000;
		this.dynamicColor = (data.dynamicColor || '');
		this.$parts = this.$container.find('.cl-itext-part');
		if (this.$parts.length == 0) return; // No animated parts
		this.parts = [];
		this.partsStates = []; // part index => text states
		this.animateParts = []; // animation index => animated parts indexes
		this.$parts.css({
			'-webkit-transition-duration': this.duration + 'ms',
			'transition-duration': this.duration + 'ms'
		}).each(function(partIndex, part){
			this.parts[partIndex] = $(part);
			this.partsStates[partIndex] = part.onclick() || [];
			this.parts[partIndex].removeAttr('onclick');
			$.map(part.className.match(/changesat_[0-9]+/g), function(elm){
				var animIndex = parseInt(elm.replace('changesat_', ''));
				if (this.animateParts[animIndex] === undefined) this.animateParts[animIndex] = [];
				this.animateParts[animIndex].push(partIndex);
			}.bind(this));
		}.bind(this));
		this.active = 0;
		this.maxActive = this.partsStates[0].length - 1;
		this._events = {
			animate: this.animate.bind(this),
			postAnimate: this.postAnimate.bind(this)
		};
		// Start animation
		this.timer = setTimeout(this._events.animate, this.delay);
	};
	CLItext.prototype = {
		animate: function(){
			var nextState = (this.active == this.maxActive) ? 0 : (this.active + 1);
			for (var partIndex = 0; partIndex < this.parts.length; partIndex++) {
				if (this.partsStates[partIndex][this.active] != this.partsStates[partIndex][nextState]) {
					this.parts[partIndex].addClass('dynamic');
					if (this.dynamicColor) {
						this.parts[partIndex].css('color', this.dynamicColor);
					}
					this._animatePart(partIndex);
				} else {
					this.parts[partIndex].removeClass('dynamic');
					if (this.dynamicColor) {
						this.parts[partIndex].css('color', '');
					}
				}
			}
			this.timer = setTimeout(this._events.postAnimate, this.duration + this.delay / 2);
		},
		/**
		 * Animate a certain dynamic part
		 * @param partIndex
		 * @private
		 */
		_animatePart: function(partIndex){
			// Preparing part for animation
			var nextState = (this.active == this.maxActive) ? 0 : (this.active + 1),
				nextValue = this.partsStates[partIndex][nextState],
				$curSpan = this.parts[partIndex].wrapInner('<span></span>').children('span'),
				$nextSpan = $('<span class="measure"></span>').html(nextValue.replace(' ', '&nbsp;')).appendTo(this.parts[partIndex]),
				nextWidth = $nextSpan.width(),
				outType = (this.type == 'flipInX') ? 'flipOutX' : 'fadeOut',
				i;
			// Measuring the future part width
			this.parts[partIndex].addClass('notransition').css('width', this.parts[partIndex].width());
			setTimeout(function(){
				this.parts[partIndex].removeClass('notransition').css('width', nextWidth);
			}.bind(this), 25);
			$curSpan.css({
				position: 'absolute',
				left: 0,
				top: 0,
				'-webkit-transition-duration': (this.duration / 5) + 'ms',
				'transition-duration': (this.duration / 5) + 'ms'
			}).addClass('animated_' + outType);
			$nextSpan.css('width', nextWidth).removeClass('measure').prependTo(this.parts[partIndex]);
			if (this.animateChars) {
				$nextSpan.empty();
				var $chars = [],
					charDuration = Math.floor(this.duration / nextValue.length);
				for (i = 0; i < nextValue.length; i++) {
					$chars.push($('<span>' + ((nextValue[i] != ' ') ? nextValue[i] : '&nbsp;') + '</span>').css({
						'-webkit-transition-duration': charDuration + 'ms',
						'transition-duration': charDuration + 'ms'
					}).appendTo($nextSpan));
				}
				$.each($chars, function(index, char){
					setTimeout(function(){
						$(char).addClass('animated_' + this.type);
					}.bind(this), charDuration * index);
				}.bind(this));
			} else {
				$nextSpan.wrapInner('<span></span>').children('span').css({
					'-webkit-transition-duration': this.duration + 'ms',
					'transition-duration': this.duration + 'ms'
				}).addClass('animated_' + this.type);
			}
			setTimeout(this._cleanupPartAnimation.bind(this, partIndex), this.duration + this.delay / 2);
		},
		/**
		 * Clean up a certain part from animation
		 * @param partIndex
		 * @private
		 */
		_cleanupPartAnimation: function(partIndex){
			var nextState = (this.active == this.maxActive) ? 0 : (this.active + 1),
				nextValue = this.partsStates[partIndex][nextState];
			this.parts[partIndex].addClass('notransition').css('width', '').html(nextValue.replace(' ', '&nbsp;'));
		},
		postAnimate: function(){
			this.active = (this.active == this.maxActive) ? 0 : (this.active + 1);
			this.timer = setTimeout(this._events.animate, this.delay / 2);
		}
	};
	if (window.$cl === undefined) window.$cl = {};
	if ($cl.elements === undefined) $cl.elements = {};
	$cl.elements['cl-itext'] = CLItext;
	if ($cl.maybeInit !== undefined) $cl.maybeInit();
}(jQuery);
