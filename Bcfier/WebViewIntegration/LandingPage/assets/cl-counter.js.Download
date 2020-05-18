/**
 * CodeLights: Counter
 */
!function($){
	/**
	 * Counter Number part animations
	 * @param container
	 * @constructor
	 */
	var CLCounterNumber = function(container){
		this.$container = $(container);
		this.initialString = this.$container.html() + '';
		this.finalString = this.$container.data('final') + '';
		this.format = this.getFormat(this.initialString, this.finalString);
		if (this.format.decMark) {
			var pattern = new RegExp('[^0-9\/' + this.format.decMark + ']+', 'g');
			this.initial = parseFloat(this.initialString.replace(pattern, '').replace(this.format.decMark, '.'));
			this.final = parseFloat(this.finalString.replace(pattern, '').replace(this.format.decMark, '.'));
		} else {
			this.initial = parseInt(this.initialString.replace(/[^0-9]+/g, ''));
			this.final = parseInt(this.finalString.replace(/[^0-9]+/g, ''));
		}
		if (this.format.accounting) {
			if (this.initialString.length > 0 && this.initialString[0] == '(') this.initial = -this.initial;
			if (this.finalString.length > 0 && this.finalString[0] == '(') this.final = -this.final;
		}
	};
	CLCounterNumber.prototype = {
		/**
		 * Function to be called at each animation frame
		 * @param now float Relative state between 0 and 1
		 */
		step: function(now){
			var value = (1 - now) * this.initial + this.final * now,
				intPart = Math[this.format.decMark ? 'floor' : 'round'](value).toString(),
				result = '';
			if (this.format.zerofill) {
				intPart = '0'.repeat(this.format.intDigits - intPart.length) + intPart;
			}
			if (this.format.groupMark) {
				if (this.format.indian) {
					result += intPart.replace(/(\d)(?=(\d\d)+\d$)/g, '$1' + this.format.groupMark);
				} else {
					result += intPart.replace(/\B(?=(\d{3})+(?!\d))/g, this.format.groupMark);
				}
			} else {
				result += intPart;
			}
			if (this.format.decMark) {
				var decimalPart = (value % 1).toFixed(this.format.decDigits).substring(2);
				result += this.format.decMark + decimalPart;
			}
			if (this.format.accounting && result.length > 0 && result[0] == '-') {
				result = '(' + result.substring(1) + ')';
			}
			this.$container.html(result);
		},
		/**
		 * Get number format based on initial and final number strings
		 * @param initial string
		 * @param final string
		 * @returns {{groupMark, decMark, accounting, zerofill, indian}}
		 */
		getFormat: function(initial, final){
			var iFormat = this._getFormat(initial),
				fFormat = this._getFormat(final),
			// Final format has more priority
				format = $.extend({}, iFormat, fFormat);
			// Group marks detector is more precise, so using it in controversial cases
			if (format.groupMark == format.decMark) delete format.groupMark;
			return format;
		},
		/**
		 * Get number format based on a single number string
		 * @param str string
		 * @returns {{groupMark, decMark, accounting, zerofill, indian}}
		 * @private
		 */
		_getFormat: function(str){
			var marks = str.replace(/[0-9\(\)\-]+/g, ''),
				format = {};
			if (str.charAt(0) == '(') format.accounting = true;
			if (/^0[0-9]/.test(str)) format.zerofill = true;
			str = str.replace(/[\(\)\-]/g, '');
			if (marks.length != 0) {
				if (marks.length > 1) {
					format.groupMark = marks.charAt(0);
					if (marks.charAt(0) != marks.charAt(marks.length - 1)) format.decMark = marks.charAt(marks.length - 1);
					if (str.split(format.groupMark).length > 2 && str.split(format.groupMark)[1].length == 2) format.indian = true;
				} else/*if (marks.length == 1)*/ {
					format[((str.length - str.indexOf(marks) - 1) == 3) ? 'groupMark' : 'decMark'] = marks;
				}
				if (format.decMark) {
					format.decDigits = str.length - str.indexOf(format.decMark) - 1;
				}
			}
			if (format.zerofill) {
				format.intDigits = str.replace(/[^\d]+/g, '').length - (format.decDigits || 0);
			}
			return format;
		}
	};

	/**
	 * Counter Number part animations
	 * @param container
	 * @constructor
	 */
	var CLCounterText = function(container){
		this.$container = $(container);
		this.initial = this.$container.text() + '';
		this.final = this.$container.data('final') + '';
		this.partsStates = this.getStates(this.initial, this.final);
		this.len = 1 / (this.partsStates.length - 1);
		// Text value won't be changed on each frame, so we'll update it only on demand
		this.curState = 0;
	};
	CLCounterText.prototype = {
		/**
		 * Function to be called at each animation frame
		 * @param now float Relative state between 0 and 1
		 */
		step: function(now){
			var state = Math.round(Math.max(0, now / this.len));
			if (state == this.curState) return;
			this.$container.html(this.partsStates[state]);
			this.curState = state;
		},
		/**
		 * Slightly modified Wagner-Fischer algorithm to obtain the shortest edit distance with intermediate states
		 * @param initial string The initial string
		 * @param final string The final string
		 * @returns {Array}
		 * @private
		 */
		getStates: function(initial, final){
			var dist = [],
				i, j;
			for (i = 0; i <= initial.length; i++) dist[i] = [i];
			for (j = 1; j <= final.length; j++) {
				dist[0][j] = j;
				for (i = 1; i <= initial.length; i++) {
					dist[i][j] = (initial[i - 1] === final[j - 1]) ? dist[i - 1][j - 1] : (Math.min(dist[i - 1][j], dist[i][j - 1], dist[i - 1][j - 1]) + 1);
				}
			}
			// Obtaining the intermediate states
			var states = [final];
			for (i = initial.length, j = final.length; i > 0 || j > 0; i--, j--) {
				var min = dist[i][j];
				if (i > 0) min = Math.min(min, dist[i - 1][j], (j > 0) ? dist[i - 1][j - 1] : min);
				if (j > 0) min = Math.min(min, dist[i][j - 1]);
				if (min >= dist[i][j]) continue;
				if (min == dist[i][j - 1]) {
					// Remove
					states.unshift(states[0].substring(0, j - 1) + states[0].substring(j));
					i++;
				} else if (min == dist[i - 1][j - 1]) {
					// Modify
					states.unshift(states[0].substring(0, j - 1) + initial[i - 1] + states[0].substring(j));
				} else if (min == dist[i - 1][j]) {
					// Insert
					states.unshift(states[0].substring(0, j) + initial[i - 1] + states[0].substring(j));
					j++;
				}
			}
			return states;
		}
	};

	/**
	 *
	 * @param container
	 * @constructor
	 */
	var CLCounter = function(container){
		// Commonly used DOM elements
		this.$container = $(container);
		this.parts = [];
		this.duration = parseInt(this.$container.data('duration') || 2000);
		this.$container.find('.cl-counter-value-part').each(function(index, part){
			var $part = $(part);
			// Skipping the ones that won't be changed
			if ($part.html() + '' == $part.data('final') + '') return;
			var type = $part.clMod('type');
			if (type == 'number') {
				this.parts.push(new CLCounterNumber($part));
			} else {
				this.parts.push(new CLCounterText($part));
			}
		}.bind(this));
		if (window.$cl !== undefined && window.$cl.scroll !== undefined) {
			// Animate element when it becomes visible
			$cl.scroll.addWaypoint(this.$container, '15%', this.animate.bind(this));
		} else {
			// No waypoints available: animate right from the start
			this.animate();
		}
	};
	CLCounter.prototype = {
		animate: function(duration){
			this.$container.css('cl-counter', 0).animate({'cl-counter': 1}, {
				duration: this.duration,
				step: this.step.bind(this)
			});
		},
		/**
		 * Function to be called at each animation frame
		 * @param now float Relative state between 0 and 1
		 */
		step: function(now){
			for (var i = 0; i < this.parts.length; i++) {
				this.parts[i].step(now);
			}
		}
	};
	if (window.$cl === undefined) window.$cl = {};
	if ($cl.elements === undefined) $cl.elements = {};
	$cl.elements['cl-counter'] = CLCounter;
	if ($cl.maybeInit !== undefined) $cl.maybeInit();
}(jQuery);
