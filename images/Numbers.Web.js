(function() {
	'use strict';
	var $asm = {};
	global.Numbers = global.Numbers || {};
	global.Numbers.Web = global.Numbers.Web || {};
	global.Numbers.Web.Controls = global.Numbers.Web.Controls || {};
	global.Numbers.Web.Generic = global.Numbers.Web.Generic || {};
	global.Numbers.Web.Transitions = global.Numbers.Web.Transitions || {};
	global.Numbers.Web.ViewModels = global.Numbers.Web.ViewModels || {};
	global.Numbers.Web.Views = global.Numbers.Web.Views || {};
	ss.initAssembly($asm, 'Numbers.Web');
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Transitions.Transition.State
	var $Numbers_$Web_Transitions_Transition$State = function() {
	};
	$Numbers_$Web_Transitions_Transition$State.__typeName = 'Numbers.$Web.Transitions.Transition$State';
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Transitions.ValueBoundsExtensions.ReversedValueBounds
	var $Numbers_$Web_Transitions_ValueBoundsExtensions$ReversedValueBounds = function(source) {
		this.$source = null;
		this.$source = source;
	};
	$Numbers_$Web_Transitions_ValueBoundsExtensions$ReversedValueBounds.__typeName = 'Numbers.$Web.Transitions.ValueBoundsExtensions$ReversedValueBounds';
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Application
	var $Numbers_Web_Application = function() {
		this.$level = 0;
		this.$game = null;
		this.$configuration = null;
		this.$customGame = false;
		this.$gameView = null;
		//
	};
	$Numbers_Web_Application.__typeName = 'Numbers.Web.Application';
	$Numbers_Web_Application.main = function() {
		var application = new $Numbers_Web_Application();
		window.addEventListener('load', function(e) {
			application.run();
		});
	};
	global.Numbers.Web.Application = $Numbers_Web_Application;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Configuration
	var $Numbers_Web_Configuration = function() {
	};
	$Numbers_Web_Configuration.__typeName = 'Numbers.Web.Configuration';
	global.Numbers.Web.Configuration = $Numbers_Web_Configuration;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.ConfigurationExtensions
	var $Numbers_Web_ConfigurationExtensions = function() {
	};
	$Numbers_Web_ConfigurationExtensions.__typeName = 'Numbers.Web.ConfigurationExtensions';
	$Numbers_Web_ConfigurationExtensions.setValue = function(configuration, key, value) {
		var $t1 = ss.today();
		configuration.setValue(key, value, new Date($t1.getFullYear() + 1, $t1.getMonth(), $t1.getDate(), $t1.getHours(), $t1.getMinutes(), $t1.getSeconds(), $t1.getMilliseconds()));
	};
	global.Numbers.Web.ConfigurationExtensions = $Numbers_Web_ConfigurationExtensions;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Game
	var $Numbers_Web_Game = function(values, targetValue) {
		this.$1$InitialValuesField = null;
		this.$1$CurrentNumbersField = null;
		this.$1$TargetValueField = 0;
		this.$stack = null;
		this.set_initialValues(Enumerable.from(values).toArray());
		this.set_currentNumbers(Enumerable.from(values).select($Numbers_Web_Number.create).toArray());
		this.set_targetValue(targetValue);
		this.$stack = new Array();
	};
	$Numbers_Web_Game.__typeName = 'Numbers.Web.Game';
	global.Numbers.Web.Game = $Numbers_Web_Game;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.GameFactory
	var $Numbers_Web_GameFactory = function() {
	};
	$Numbers_Web_GameFactory.__typeName = 'Numbers.Web.GameFactory';
	$Numbers_Web_GameFactory.createFromHash = function(value) {
		var values = Enumerable.from(value.split('-')).select(function(valueString) {
			return parseInt(valueString);
		}).take(7).toArray();
		return new $Numbers_Web_Game(Enumerable.from(values).take(Enumerable.from(values).count() - 1), Enumerable.from(values).last());
	};
	$Numbers_Web_GameFactory.createFromSolutionRange = function(minimumSolutions, maximumSolutions) {
		console.log(ss.formatString('Creating a game with {0}-{1} solutions', minimumSolutions, maximumSolutions));
		while (true) {
			var values = $Numbers_Web_GameFactory.$generateRandomValues();
			var target = $Numbers_Web_GameFactory.$getRandomTarget(values, minimumSolutions, maximumSolutions);
			if (target !== -1) {
				return new $Numbers_Web_Game(values, target);
			}
		}
	};
	$Numbers_Web_GameFactory.$getRandomTarget = function(values, minimumSolutions, maximumSolutions) {
		var solutionsCount = $Numbers_Web_Solver.getSolutionsCount(values, $Numbers_Web_GameFactory.$maximumTarget);
		var targets = Enumerable.from(solutionsCount).select(function(count, target) {
			return { item1: count, item2: target };
		}).where(function(tuple) {
			return tuple.item1 >= minimumSolutions && tuple.item1 <= maximumSolutions;
		}).select(function(tuple1) {
			return tuple1.item2;
		}).where(function(target1) {
			return target1 >= $Numbers_Web_GameFactory.$minimumTarget;
		}).toArray();
		if (targets.length === 0) {
			return -1;
		}
		var randomTarget = ss.Int32.trunc($Numbers_Web_GameFactory.$getNormalDistributedRandom(200, 100));
		return Enumerable.from(targets).orderBy(function(target2) {
			return Math.abs(target2 - randomTarget);
		}).first();
	};
	$Numbers_Web_GameFactory.$getNormalDistributedRandom = function(mean, sd) {
		//Box-Muller transform
		var normalizedValue = Math.sqrt(-2 * Math.log($Numbers_Web_GameFactory.$random.nextDouble())) * Math.sin(2 * Math.PI * $Numbers_Web_GameFactory.$random.nextDouble());
		return mean + sd * normalizedValue;
	};
	$Numbers_Web_GameFactory.$generateRandomValues = function() {
		var values = [];
		var value = 0;
		ss.add(values, value += 1 + $Numbers_Web_GameFactory.$random.nextMax(2));
		ss.add(values, value += 1 + $Numbers_Web_GameFactory.$random.nextMax(3));
		ss.add(values, value += 1 + $Numbers_Web_GameFactory.$random.nextMax(4));
		ss.add(values, value += 1 + $Numbers_Web_GameFactory.$random.nextMax(7));
		ss.add(values, value += 1 + $Numbers_Web_GameFactory.$random.nextMax(12));
		ss.add(values, value += 1 + $Numbers_Web_GameFactory.$random.nextMax(20));
		return values;
	};
	global.Numbers.Web.GameFactory = $Numbers_Web_GameFactory;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.IConfiguration
	var $Numbers_Web_IConfiguration = function() {
	};
	$Numbers_Web_IConfiguration.__typeName = 'Numbers.Web.IConfiguration';
	global.Numbers.Web.IConfiguration = $Numbers_Web_IConfiguration;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.IGameHost
	var $Numbers_Web_IGameHost = function() {
	};
	$Numbers_Web_IGameHost.__typeName = 'Numbers.Web.IGameHost';
	global.Numbers.Web.IGameHost = $Numbers_Web_IGameHost;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.LevelChange
	var $Numbers_Web_LevelChange = function() {
	};
	$Numbers_Web_LevelChange.__typeName = 'Numbers.Web.LevelChange';
	global.Numbers.Web.LevelChange = $Numbers_Web_LevelChange;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Number
	var $Numbers_Web_Number = function(value, level, operand1, operand2, operator) {
		this.$1$ValueField = 0;
		this.$1$LevelField = 0;
		this.$1$Operand1Field = null;
		this.$1$Operand2Field = null;
		this.$1$OperatorField = 0;
		this.set_value(value);
		this.set_level(level);
		this.set_operand1(operand1);
		this.set_operand2(operand2);
		this.set_operator(operator);
	};
	$Numbers_Web_Number.__typeName = 'Numbers.Web.Number';
	$Numbers_Web_Number.op_Equality = function(a, b) {
		return ss.referenceEquals(a, null) && ss.referenceEquals(b, null) || !ss.referenceEquals(a, null) && a.equals(b);
	};
	$Numbers_Web_Number.op_Inequality = function(a, b) {
		return !$Numbers_Web_Number.op_Equality(a, b);
	};
	$Numbers_Web_Number.create = function(value) {
		return new $Numbers_Web_Number(value, 1, null, null, 0);
	};
	$Numbers_Web_Number.add = function(a, b) {
		return new $Numbers_Web_Number(a.get_value() + b.get_value(), a.get_level() + b.get_level(), a, b, 1);
	};
	$Numbers_Web_Number.subtract = function(a, b) {
		if (a.get_value() < b.get_value()) {
			var c = a;
			a = b;
			b = c;
		}
		return new $Numbers_Web_Number(a.get_value() - b.get_value(), a.get_level() + b.get_level(), a, b, 2);
	};
	$Numbers_Web_Number.multiply = function(a, b) {
		return new $Numbers_Web_Number(a.get_value() * b.get_value(), a.get_level() + b.get_level(), a, b, 3);
	};
	$Numbers_Web_Number.divide = function(a, b) {
		if (a.get_value() < b.get_value()) {
			var c = a;
			a = b;
			b = c;
		}
		if (b.get_value() === 0 || a.get_value() % b.get_value() !== 0) {
			return null;
		}
		return new $Numbers_Web_Number(((a.get_value() > b.get_value()) ? ss.Int32.div(a.get_value(), b.get_value()) : ss.Int32.div(b.get_value(), a.get_value())), a.get_level() + b.get_level(), a, b, 4);
	};
	$Numbers_Web_Number.$getOperatorString = function(value) {
		switch (value) {
			case 0: {
				return 'Create';
			}
			case 1: {
				return '+';
			}
			case 2: {
				return '-';
			}
			case 3: {
				return '*';
			}
			case 4: {
				return '/';
			}
			default: {
				throw new ss.Exception('Unrecognized Operator');
			}
		}
	};
	global.Numbers.Web.Number = $Numbers_Web_Number;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Operator
	var $Numbers_Web_Operator = function() {
	};
	$Numbers_Web_Operator.__typeName = 'Numbers.Web.Operator';
	global.Numbers.Web.Operator = $Numbers_Web_Operator;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Solver
	var $Numbers_Web_Solver = function() {
	};
	$Numbers_Web_Solver.__typeName = 'Numbers.Web.Solver';
	$Numbers_Web_Solver.getSolutionsCount = function(values, maximumTarget) {
		if (Enumerable.from(values).distinct().count() !== Enumerable.from(values).count()) {
			throw new ss.Exception('Values are not distinct');
		}
		var solutionsCount = { $: ss.repeat(0, maximumTarget) };
		$Numbers_Web_Solver.$countSolutions(Enumerable.from(values).select($Numbers_Web_Number.create).toArray(), 0, solutionsCount);
		return solutionsCount.$;
	};
	$Numbers_Web_Solver.$countSolutions = function(numbers, startIndex, solutionsCount) {
		if (numbers.length === 1) {
			var target = numbers[0].get_value();
			if (target < solutionsCount.$.length) {
				solutionsCount.$[target]++;
			}
			return;
		}
		var newNumbers = { $: new Array(numbers.length - 1) };
		for (var i = 0; i < numbers.length - 1; i++) {
			for (var j = i + 1; j < numbers.length; j++) {
				// combinations of [i,j] below startIndex have already been checked earlier in the recursion
				// except [i,j=last] (because the last number is a new result which only exists in the current iteration)
				if (i < startIndex && j < numbers.length - 1) {
					continue;
				}
				$Numbers_Web_Solver.$copyPartialArray($Numbers_Web_Number).call(null, numbers, newNumbers, [i, j]);
				var $t1 = ss.getEnumerator($Numbers_Web_Solver.$getNumbersOperations(numbers[i], numbers[j]));
				try {
					while ($t1.moveNext()) {
						var nextNumber = $t1.current();
						if ($Numbers_Web_Number.op_Equality(nextNumber, null)) {
							continue;
						}
						newNumbers.$[newNumbers.$.length - 1] = nextNumber;
						$Numbers_Web_Solver.$countSolutions(newNumbers.$, Math.max(startIndex, i), solutionsCount);
					}
				}
				finally {
					$t1.dispose();
				}
			}
		}
	};
	$Numbers_Web_Solver.$getNumbersOperations = function(number1, number2) {
		return new ss.IteratorBlockEnumerable(function() {
			return (function(number1, number2) {
				var $result, $state = 0, number3;
				return new ss.IteratorBlockEnumerator(function() {
					$sm1:
					for (;;) {
						switch ($state) {
							case 0: {
								$state = -1;
								if (number1.compareTo(number2) < 0) {
									number3 = number1;
									number1 = number2;
									number2 = number3;
								}
								// check only these patterns (where a > b > c) and skip any other combination:
								// (a+b)+c, (a+b)-c, (a-b)+c, (a-b)-c (when c is not (d+e) or (d-e))
								// (a*b)*c, (a*b)/c, (a/b)*c, (a/b)/c (when c is not (d*e) or (d/e))
								if ((number1.get_operator() !== 1 && number1.get_operator() !== 2 || number1.get_operand1().compareTo(number2) > 0 && number1.get_operand2().compareTo(number2) > 0) && number2.get_operator() !== 1 && number2.get_operator() !== 2) {
									$result = $Numbers_Web_Number.add(number1, number2);
									$state = 2;
									return true;
								}
								$state = 1;
								continue $sm1;
							}
							case 2: {
								$state = -1;
								if (number2.get_value() !== 0) {
									$result = $Numbers_Web_Number.subtract(number1, number2);
									$state = 1;
									return true;
								}
								$state = 1;
								continue $sm1;
							}
							case 1: {
								$state = -1;
								if ((number1.get_operator() !== 3 && number1.get_operator() !== 4 || number1.get_operand1().compareTo(number2) > 0 && number1.get_operand2().compareTo(number2) > 0) && number2.get_operator() !== 3 && number2.get_operator() !== 4) {
									$result = $Numbers_Web_Number.multiply(number1, number2);
									$state = 3;
									return true;
								}
								$state = -1;
								break $sm1;
							}
							case 3: {
								$state = -1;
								if (number2.get_value() !== 1) {
									$result = $Numbers_Web_Number.divide(number1, number2);
									$state = -1;
									return true;
								}
								$state = -1;
								break $sm1;
							}
							default: {
								break $sm1;
							}
						}
					}
					return false;
				}, function() {
					return $result;
				}, null, this);
			}).call(this, number1, number2);
		}, this);
	};
	$Numbers_Web_Solver.findInitialOperation = function(target, initialNumbers) {
		return $Numbers_Web_Solver.$findInitialOperation(target, Enumerable.from(initialNumbers).toArray());
	};
	$Numbers_Web_Solver.$findInitialOperation = function(target, initialNumbers) {
		if ($Numbers_Web_Number.op_Equality(target, null)) {
			return null;
		}
		if (ss.contains(initialNumbers, target.get_operand1()) && ss.contains(initialNumbers, target.get_operand2())) {
			return target;
		}
		return $Numbers_Web_Solver.$findInitialOperation(target.get_operand1(), initialNumbers) || $Numbers_Web_Solver.$findInitialOperation(target.get_operand2(), initialNumbers);
	};
	$Numbers_Web_Solver.findSolution = function(numbers, target) {
		return $Numbers_Web_Solver.$findSolution(Enumerable.from(numbers).toArray(), target);
	};
	$Numbers_Web_Solver.$findSolution = function(numbers, target) {
		if (numbers.length === 1) {
			return ((numbers[0].get_value() === target) ? numbers[0] : null);
		}
		var newNumbers = { $: new Array(numbers.length - 1) };
		for (var i = 0; i < numbers.length; i++) {
			for (var j = 0; j < numbers.length; j++) {
				var number1 = numbers[i];
				var number2 = numbers[j];
				if (i === j || number1.get_value() < number2.get_value()) {
					continue;
				}
				$Numbers_Web_Solver.$copyPartialArray($Numbers_Web_Number).call(null, numbers, newNumbers, [i, j]);
				var nextNumbers = [$Numbers_Web_Number.add(number1, number2), $Numbers_Web_Number.subtract(number1, number2), $Numbers_Web_Number.multiply(number1, number2), $Numbers_Web_Number.divide(number1, number2)];
				for (var $t1 = 0; $t1 < nextNumbers.length; $t1++) {
					var nextNumber = nextNumbers[$t1];
					if ($Numbers_Web_Number.op_Equality(nextNumber, null)) {
						continue;
					}
					newNumbers.$[newNumbers.$.length - 1] = nextNumber;
					var result = $Numbers_Web_Solver.$findSolution(newNumbers.$, target);
					if ($Numbers_Web_Number.op_Inequality(result, null)) {
						return result;
					}
				}
			}
		}
		return null;
	};
	$Numbers_Web_Solver.$copyPartialArray = function(T) {
		return function(source, target, excludeIndexes) {
			var resultIndex = 0;
			for (var i = 0; i < source.length; i++) {
				if (ss.contains(excludeIndexes, i)) {
					continue;
				}
				target.$[resultIndex] = source[i];
				resultIndex++;
			}
		};
	};
	global.Numbers.Web.Solver = $Numbers_Web_Solver;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.StyleExtensions
	var $Numbers_Web_StyleExtensions = function() {
	};
	$Numbers_Web_StyleExtensions.__typeName = 'Numbers.Web.StyleExtensions';
	$Numbers_Web_StyleExtensions.getTransitionDictionary = function(style) {
		return new $Numbers_Web_TokenDictionary(function() {
			return style.transition;
		}, function(value) {
			style.transition = value;
			null;
		});
	};
	global.Numbers.Web.StyleExtensions = $Numbers_Web_StyleExtensions;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.TokenDictionary
	var $Numbers_Web_TokenDictionary = function(getRawList, setRawList) {
		this.$getRawList = null;
		this.$setRawList = null;
		this.$getRawList = getRawList;
		this.$setRawList = setRawList;
	};
	$Numbers_Web_TokenDictionary.__typeName = 'Numbers.Web.TokenDictionary';
	$Numbers_Web_TokenDictionary.$createDictionary = function(rawList) {
		return Enumerable.from(rawList.split(String.fromCharCode($Numbers_Web_TokenDictionary.$tokenSeparator))).where(function(token) {
			return !ss.isNullOrEmptyString(token);
		}).toDictionary($Numbers_Web_TokenDictionary.$getTokenKey, $Numbers_Web_TokenDictionary.$getTokenValue, String, String);
	};
	$Numbers_Web_TokenDictionary.$createRawList = function(dictionary) {
		return Enumerable.from(dictionary.get_keys()).select(function(key) {
			return ss.formatString('{0}{1}{2}', key, String.fromCharCode($Numbers_Web_TokenDictionary.$keyValueSeparator), dictionary.get_item(key));
		}).defaultIfEmpty('').aggregate(function(s1, s2) {
			return ss.formatString('{0}{1} {2}', s1, String.fromCharCode($Numbers_Web_TokenDictionary.$tokenSeparator), s2);
		});
	};
	$Numbers_Web_TokenDictionary.$getTokenKey = function(token) {
		token = ss.trimStartString(token);
		var index = token.indexOf(String.fromCharCode($Numbers_Web_TokenDictionary.$keyValueSeparator));
		return ((index === -1) ? token : token.substr(0, index));
	};
	$Numbers_Web_TokenDictionary.$getTokenValue = function(token) {
		token = ss.trimStartString(token);
		var index = token.indexOf(String.fromCharCode($Numbers_Web_TokenDictionary.$keyValueSeparator));
		return ((index === -1) ? '' : ss.trimStartString(token.substring(index + 1), [$Numbers_Web_TokenDictionary.$keyValueSeparator]));
	};
	global.Numbers.Web.TokenDictionary = $Numbers_Web_TokenDictionary;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Controls.Button
	var $Numbers_Web_Controls_Button = function(initialCheck, classesName) {
		this.$2$IsEnabledField = false;
		this.$2$IsCheckedChangedField = null;
		this.$isChecked = false;
		this.$2$ShadowField = null;
		this.$overlay = null;
		this.$checkedAnimation = null;
		this.$uncheckedAnimation = null;
		this.$overlayAnimation = null;
		$Numbers_Web_Controls_Control.call(this, ['button']);
		this.$isChecked = initialCheck;
		this.set_isEnabled(true);
		for (var $t1 = 0; $t1 < classesName.length; $t1++) {
			var className = classesName[$t1];
			this.get_htmlElement().classList.add(className);
		}
		this.set_shadow(new $Numbers_Web_Controls_Control(['button-shadow']));
		this.$overlay = new $Numbers_Web_Controls_Control(['button-overlay']);
		this.appendChild(this.$overlay);
		this.get_htmlElement().addEventListener('mousedown', ss.mkdel(this, this.$onMouseDown), false);
		var transformValueBounds = new $Numbers_Web_Transitions_ScaleValueBounds(1, 1.06);
		var marginValueBounds = new $Numbers_Web_Transitions_PixelValueBounds(0, 4);
		var opacityValueBounds = new $Numbers_Web_Transitions_DoubleValueBounds(0, 1);
		var transitionTiming = new $Numbers_Web_Transitions_TransitionTiming($Numbers_Web_Controls_Button.$checkAnimationDurationMilliseconds, null, 0);
		this.$checkedAnimation = new $Numbers_Web_Transitions_ParallelTransition([new $Numbers_Web_Transitions_MultiplePropertyTransition(this.get_htmlElement(), ['transform', '-webkit-transform'], transformValueBounds, transitionTiming, 0, 0), new $Numbers_Web_Transitions_Transition(this.get_htmlElement(), 'marginTop', marginValueBounds, transitionTiming, 0, 0), new $Numbers_Web_Transitions_MultiplePropertyTransition(this.get_shadow().get_htmlElement(), ['transform', '-webkit-transform'], transformValueBounds, transitionTiming, 0, 0), new $Numbers_Web_Transitions_Transition(this.get_shadow().get_htmlElement(), 'marginTop', marginValueBounds, transitionTiming, 0, 0), new $Numbers_Web_Transitions_Transition(this.get_shadow().get_htmlElement(), 'opacity', opacityValueBounds, transitionTiming, 0, 0)]);
		this.$uncheckedAnimation = new $Numbers_Web_Transitions_ParallelTransition([new $Numbers_Web_Transitions_MultiplePropertyTransition(this.get_htmlElement(), ['transform', '-webkit-transform'], $Numbers_Web_Transitions_ValueBoundsExtensions.reverse(transformValueBounds), transitionTiming, 0, 0), new $Numbers_Web_Transitions_Transition(this.get_htmlElement(), 'marginTop', $Numbers_Web_Transitions_ValueBoundsExtensions.reverse(marginValueBounds), transitionTiming, 0, 0), new $Numbers_Web_Transitions_MultiplePropertyTransition(this.get_shadow().get_htmlElement(), ['transform', '-webkit-transform'], $Numbers_Web_Transitions_ValueBoundsExtensions.reverse(transformValueBounds), transitionTiming, 0, 0), new $Numbers_Web_Transitions_Transition(this.get_shadow().get_htmlElement(), 'marginTop', $Numbers_Web_Transitions_ValueBoundsExtensions.reverse(marginValueBounds), transitionTiming, 0, 0), new $Numbers_Web_Transitions_Transition(this.get_shadow().get_htmlElement(), 'opacity', $Numbers_Web_Transitions_ValueBoundsExtensions.reverse(opacityValueBounds), transitionTiming, 0, 0)]);
		this.$overlayAnimation = new $Numbers_Web_Transitions_ParallelTransition([new $Numbers_Web_Transitions_MultiplePropertyTransition(this.$overlay.get_htmlElement(), ['transform', '-webkit-transform'], new $Numbers_Web_Transitions_ScaleValueBounds(0, 1.5), new $Numbers_Web_Transitions_TransitionTiming(400, $Numbers_Web_Transitions_TimingCurve.easeOut, 0), 0, 0), new $Numbers_Web_Transitions_SequentialTransition([new $Numbers_Web_Transitions_Transition(this.$overlay.get_htmlElement(), 'opacity', new $Numbers_Web_Transitions_DoubleValueBounds(0, 1), new $Numbers_Web_Transitions_TransitionTiming(100, $Numbers_Web_Transitions_TimingCurve.easeIn, 0), 0, 2), new $Numbers_Web_Transitions_Transition(this.$overlay.get_htmlElement(), 'opacity', new $Numbers_Web_Transitions_DoubleValueBounds(1, 0), new $Numbers_Web_Transitions_TransitionTiming(300, $Numbers_Web_Transitions_TimingCurve.easeOut, 0), 0, 2)])]);
		if (this.get_isChecked()) {
			this.get_htmlElement().style['transform'] = transformValueBounds.get_formattedEndValue();
			this.get_htmlElement().style['-webkit-transform'] = transformValueBounds.get_formattedEndValue();
			this.get_htmlElement().style['marginTop'] = marginValueBounds.get_formattedEndValue();
			this.get_shadow().get_htmlElement().style['transform'] = transformValueBounds.get_formattedEndValue();
			this.get_shadow().get_htmlElement().style['-webkit-transform'] = transformValueBounds.get_formattedEndValue();
			this.get_shadow().get_htmlElement().style['marginTop'] = marginValueBounds.get_formattedEndValue();
		}
	};
	$Numbers_Web_Controls_Button.__typeName = 'Numbers.Web.Controls.Button';
	global.Numbers.Web.Controls.Button = $Numbers_Web_Controls_Button;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Controls.Control
	var $Numbers_Web_Controls_Control = function(classesName) {
		this.$1$HtmlElementField = null;
		this.$left = 0;
		this.$top = 0;
		this.$children = null;
		this.set_htmlElement(document.createElement('div'));
		this.$children = [];
		for (var $t1 = 0; $t1 < classesName.length; $t1++) {
			var className = classesName[$t1];
			if (!ss.isNullOrEmptyString(className)) {
				this.get_htmlElement().classList.add(className);
			}
		}
	};
	$Numbers_Web_Controls_Control.__typeName = 'Numbers.Web.Controls.Control';
	global.Numbers.Web.Controls.Control = $Numbers_Web_Controls_Control;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Controls.Label
	var $Numbers_Web_Controls_Label = function(className) {
		$Numbers_Web_Controls_Control.call(this, ['label', className]);
		//
	};
	$Numbers_Web_Controls_Label.__typeName = 'Numbers.Web.Controls.Label';
	global.Numbers.Web.Controls.Label = $Numbers_Web_Controls_Label;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Controls.ToolbarButton
	var $Numbers_Web_Controls_ToolbarButton = function(className, imageSource, clicked) {
		this.$2$IsEnabledField = false;
		$Numbers_Web_Controls_Control.call(this, ['toolbar-button', className]);
		var imageElement = document.createElement('img');
		imageElement.setAttribute('src', imageSource);
		this.get_htmlElement().appendChild(imageElement);
		this.appendChild(new $Numbers_Web_Controls_Control(['toolbar-button-overlay']));
		this.set_isEnabled(true);
		this.get_htmlElement().addEventListener('mousedown', ss.mkdel(this, function(e) {
			if (this.get_isEnabled()) {
				clicked();
			}
		}), false);
	};
	$Numbers_Web_Controls_ToolbarButton.__typeName = 'Numbers.Web.Controls.ToolbarButton';
	global.Numbers.Web.Controls.ToolbarButton = $Numbers_Web_Controls_ToolbarButton;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Generic.ConvertedObservableCollection
	var $Numbers_Web_Generic_ConvertedObservableCollection$2 = function(S, T) {
		var $type = function(sourceCollection, convertItem, detachItem) {
			this.$1$CollectionChangedField = null;
			this.$sourceCollection = null;
			this.$convertedCollection = null;
			this.$convertItem = null;
			this.$detachItem = null;
			this.$sourceCollection = sourceCollection;
			this.$convertedCollection = new (ss.makeGenericType($Numbers_Web_Generic_ObservableCollection$1, [T]))();
			this.$convertItem = convertItem;
			this.$detachItem = detachItem;
			this.$build();
			sourceCollection.add_collectionChanged(ss.mkdel(this, this.$sourceCollectionChanged));
			this.$convertedCollection.add_collectionChanged(ss.mkdel(this, this.$convertedCollectionChanged));
		};
		ss.registerGenericClassInstance($type, $Numbers_Web_Generic_ConvertedObservableCollection$2, [S, T], {
			add_collectionChanged: function(value) {
				this.$1$CollectionChangedField = ss.delegateCombine(this.$1$CollectionChangedField, value);
			},
			remove_collectionChanged: function(value) {
				this.$1$CollectionChangedField = ss.delegateRemove(this.$1$CollectionChangedField, value);
			},
			$build: function() {
				var $t1 = ss.getEnumerator(this.$sourceCollection);
				try {
					while ($t1.moveNext()) {
						var sourceItem = $t1.current();
						this.$convertedCollection.add(this.$convertItem(sourceItem));
					}
				}
				finally {
					$t1.dispose();
				}
			},
			$clear: function() {
				if (!ss.staticEquals(this.$detachItem, null)) {
					var $t1 = this.$convertedCollection.getEnumerator();
					try {
						while ($t1.moveNext()) {
							var convertedItem = $t1.current();
							this.$detachItem(convertedItem);
						}
					}
					finally {
						$t1.dispose();
					}
				}
				this.$convertedCollection.clear();
			},
			$sourceCollectionChanged: function(sender, eventArgs) {
				if (eventArgs.get_action() === 0) {
					this.$convertedCollection.insert(eventArgs.get_index(), this.$convertItem(ss.cast(eventArgs.get_item(), S)));
				}
				if (eventArgs.get_action() === 1) {
					if (!ss.staticEquals(this.$detachItem, null)) {
						this.$detachItem(this.$convertedCollection.get_item(eventArgs.get_index()));
					}
					this.$convertedCollection.removeAt(eventArgs.get_index());
				}
				if (eventArgs.get_action() === 2) {
					this.$clear();
					this.$build();
				}
			},
			$convertedCollectionChanged: function(sender, eventArgs) {
				if (!ss.staticEquals(this.$1$CollectionChangedField, null)) {
					this.$1$CollectionChangedField(this, eventArgs);
				}
			},
			getEnumerator: function() {
				return this.$convertedCollection.getEnumerator();
			}
		}, function() {
			return null;
		}, function() {
			return [ss.IEnumerable, ss.IEnumerable, $Numbers_Web_Generic_INotifyCollectionChanged, ss.makeGenericType($Numbers_Web_Generic_IObservableEnumerable$1, [T])];
		});
		return $type;
	};
	$Numbers_Web_Generic_ConvertedObservableCollection$2.__typeName = 'Numbers.Web.Generic.ConvertedObservableCollection$2';
	ss.initGenericClass($Numbers_Web_Generic_ConvertedObservableCollection$2, $asm, 2);
	global.Numbers.Web.Generic.ConvertedObservableCollection$2 = $Numbers_Web_Generic_ConvertedObservableCollection$2;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Generic.INotifyCollectionChanged
	var $Numbers_Web_Generic_INotifyCollectionChanged = function() {
	};
	$Numbers_Web_Generic_INotifyCollectionChanged.__typeName = 'Numbers.Web.Generic.INotifyCollectionChanged';
	global.Numbers.Web.Generic.INotifyCollectionChanged = $Numbers_Web_Generic_INotifyCollectionChanged;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Generic.INotifyPropertyChanged
	var $Numbers_Web_Generic_INotifyPropertyChanged = function() {
	};
	$Numbers_Web_Generic_INotifyPropertyChanged.__typeName = 'Numbers.Web.Generic.INotifyPropertyChanged';
	global.Numbers.Web.Generic.INotifyPropertyChanged = $Numbers_Web_Generic_INotifyPropertyChanged;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Generic.IObservableCollection
	var $Numbers_Web_Generic_IObservableCollection$1 = function(T) {
		var $type = function() {
		};
		ss.registerGenericInterfaceInstance($type, $Numbers_Web_Generic_IObservableCollection$1, [T], {}, function() {
			return [ss.IEnumerable, ss.IEnumerable, $Numbers_Web_Generic_INotifyCollectionChanged, ss.makeGenericType($Numbers_Web_Generic_IObservableEnumerable$1, [T]), ss.ICollection, ss.IList];
		});
		return $type;
	};
	$Numbers_Web_Generic_IObservableCollection$1.__typeName = 'Numbers.Web.Generic.IObservableCollection$1';
	ss.initGenericInterface($Numbers_Web_Generic_IObservableCollection$1, $asm, 1);
	global.Numbers.Web.Generic.IObservableCollection$1 = $Numbers_Web_Generic_IObservableCollection$1;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Generic.IObservableEnumerable
	var $Numbers_Web_Generic_IObservableEnumerable$1 = function(T) {
		var $type = function() {
		};
		ss.registerGenericInterfaceInstance($type, $Numbers_Web_Generic_IObservableEnumerable$1, [T], {}, function() {
			return [ss.IEnumerable, ss.IEnumerable, $Numbers_Web_Generic_INotifyCollectionChanged];
		});
		return $type;
	};
	$Numbers_Web_Generic_IObservableEnumerable$1.__typeName = 'Numbers.Web.Generic.IObservableEnumerable$1';
	ss.initGenericInterface($Numbers_Web_Generic_IObservableEnumerable$1, $asm, 1);
	global.Numbers.Web.Generic.IObservableEnumerable$1 = $Numbers_Web_Generic_IObservableEnumerable$1;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Generic.NotifyCollectionChangedAction
	var $Numbers_Web_Generic_NotifyCollectionChangedAction = function() {
	};
	$Numbers_Web_Generic_NotifyCollectionChangedAction.__typeName = 'Numbers.Web.Generic.NotifyCollectionChangedAction';
	global.Numbers.Web.Generic.NotifyCollectionChangedAction = $Numbers_Web_Generic_NotifyCollectionChangedAction;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Generic.NotifyCollectionChangedEventArgs
	var $Numbers_Web_Generic_NotifyCollectionChangedEventArgs = function(action, item, index) {
		this.$2$ActionField = 0;
		this.$2$ItemField = null;
		this.$2$IndexField = 0;
		ss.EventArgs.call(this);
		this.set_action(action);
		this.set_item(item);
		this.set_index(index);
	};
	$Numbers_Web_Generic_NotifyCollectionChangedEventArgs.__typeName = 'Numbers.Web.Generic.NotifyCollectionChangedEventArgs';
	$Numbers_Web_Generic_NotifyCollectionChangedEventArgs.createAdd = function(item, index) {
		return new $Numbers_Web_Generic_NotifyCollectionChangedEventArgs(0, item, index);
	};
	$Numbers_Web_Generic_NotifyCollectionChangedEventArgs.createRemove = function(item, index) {
		return new $Numbers_Web_Generic_NotifyCollectionChangedEventArgs(1, item, index);
	};
	$Numbers_Web_Generic_NotifyCollectionChangedEventArgs.createReset = function() {
		return new $Numbers_Web_Generic_NotifyCollectionChangedEventArgs(2, null, -1);
	};
	global.Numbers.Web.Generic.NotifyCollectionChangedEventArgs = $Numbers_Web_Generic_NotifyCollectionChangedEventArgs;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Generic.ObservableCollection
	var $Numbers_Web_Generic_ObservableCollection$1 = function(T) {
		var $type = function() {
			this.$1$CollectionChangedField = null;
			this.$1$PropertyChangedField = null;
			this.$items = null;
			this.$items = [];
		};
		ss.registerGenericClassInstance($type, $Numbers_Web_Generic_ObservableCollection$1, [T], {
			add_collectionChanged: function(value) {
				this.$1$CollectionChangedField = ss.delegateCombine(this.$1$CollectionChangedField, value);
			},
			remove_collectionChanged: function(value) {
				this.$1$CollectionChangedField = ss.delegateRemove(this.$1$CollectionChangedField, value);
			},
			add_propertyChanged: function(value) {
				this.$1$PropertyChangedField = ss.delegateCombine(this.$1$PropertyChangedField, value);
			},
			remove_propertyChanged: function(value) {
				this.$1$PropertyChangedField = ss.delegateRemove(this.$1$PropertyChangedField, value);
			},
			get_count: function() {
				return this.$items.length;
			},
			get_item: function(index) {
				return this.$items[index];
			},
			set_item: function(index, value) {
				this.$raiseCollectionChanged($Numbers_Web_Generic_NotifyCollectionChangedEventArgs.createRemove(this.$items[index], index));
				this.$items[index] = value;
				this.$raiseCollectionChanged($Numbers_Web_Generic_NotifyCollectionChangedEventArgs.createAdd(value, index));
			},
			add: function(item) {
				ss.add(this.$items, item);
				this.$raiseCollectionChanged($Numbers_Web_Generic_NotifyCollectionChangedEventArgs.createAdd(item, this.get_count() - 1));
				this.$raisePropertyChanged($type.$countPropertyChangedEventArgs);
			},
			insert: function(index, item) {
				if (index > this.$items.length) {
					this.add(item);
				}
				else {
					if (index < 0) {
						index = 0;
					}
					ss.insert(this.$items, index, item);
					this.$raiseCollectionChanged($Numbers_Web_Generic_NotifyCollectionChangedEventArgs.createAdd(item, index));
					this.$raisePropertyChanged($type.$countPropertyChangedEventArgs);
				}
			},
			remove: function(item) {
				var index = ss.indexOf(this.$items, item);
				if (index === -1) {
					return false;
				}
				ss.remove(this.$items, item);
				this.$raiseCollectionChanged($Numbers_Web_Generic_NotifyCollectionChangedEventArgs.createRemove(item, index));
				this.$raisePropertyChanged($type.$countPropertyChangedEventArgs);
				return true;
			},
			removeAt: function(index) {
				var item = this.$items[index];
				ss.removeAt(this.$items, index);
				this.$raiseCollectionChanged($Numbers_Web_Generic_NotifyCollectionChangedEventArgs.createRemove(item, index));
				this.$raisePropertyChanged($type.$countPropertyChangedEventArgs);
			},
			clear: function() {
				if (this.get_count() > 0) {
					ss.clear(this.$items);
					this.$raiseCollectionChanged($Numbers_Web_Generic_NotifyCollectionChangedEventArgs.createReset());
					this.$raisePropertyChanged($type.$countPropertyChangedEventArgs);
				}
			},
			contains: function(item) {
				return ss.contains(this.$items, item);
			},
			indexOf: function(item) {
				return ss.indexOf(this.$items, item);
			},
			getEnumerator: function() {
				return ss.getEnumerator(this.$items);
			},
			$raisePropertyChanged: function(e) {
				if (!ss.staticEquals(this.$1$PropertyChangedField, null)) {
					this.$1$PropertyChangedField(this, e);
				}
			},
			$raiseCollectionChanged: function(e) {
				if (!ss.staticEquals(this.$1$CollectionChangedField, null)) {
					this.$1$CollectionChangedField(this, e);
				}
			}
		}, function() {
			return null;
		}, function() {
			return [ss.IEnumerable, ss.IEnumerable, $Numbers_Web_Generic_INotifyCollectionChanged, ss.makeGenericType($Numbers_Web_Generic_IObservableEnumerable$1, [T]), ss.ICollection, ss.IList, ss.makeGenericType($Numbers_Web_Generic_IObservableCollection$1, [T]), $Numbers_Web_Generic_INotifyPropertyChanged];
		});
		$type.$countPropertyChangedEventArgs = new $Numbers_Web_Generic_PropertyChangedEventArgs('Count');
		return $type;
	};
	$Numbers_Web_Generic_ObservableCollection$1.__typeName = 'Numbers.Web.Generic.ObservableCollection$1';
	ss.initGenericClass($Numbers_Web_Generic_ObservableCollection$1, $asm, 1);
	global.Numbers.Web.Generic.ObservableCollection$1 = $Numbers_Web_Generic_ObservableCollection$1;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Generic.PropertyChangedEventArgs
	var $Numbers_Web_Generic_PropertyChangedEventArgs = function(propertyName) {
		this.$2$PropertyNameField = null;
		ss.EventArgs.call(this);
		this.set_propertyName(propertyName);
	};
	$Numbers_Web_Generic_PropertyChangedEventArgs.__typeName = 'Numbers.Web.Generic.PropertyChangedEventArgs';
	global.Numbers.Web.Generic.PropertyChangedEventArgs = $Numbers_Web_Generic_PropertyChangedEventArgs;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Transitions.DoubleValueBounds
	var $Numbers_Web_Transitions_DoubleValueBounds = function(startValue, endValue) {
		this.$1$FormattedStartValueField = null;
		this.$1$FormattedEndValueField = null;
		this.$startValue = 0;
		this.$endValue = 0;
		this.$startValue = startValue;
		this.$endValue = endValue;
		this.set_formattedStartValue(startValue.toString());
		this.set_formattedEndValue(endValue.toString());
	};
	$Numbers_Web_Transitions_DoubleValueBounds.__typeName = 'Numbers.Web.Transitions.DoubleValueBounds';
	global.Numbers.Web.Transitions.DoubleValueBounds = $Numbers_Web_Transitions_DoubleValueBounds;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Transitions.ITransition
	var $Numbers_Web_Transitions_ITransition = function() {
	};
	$Numbers_Web_Transitions_ITransition.__typeName = 'Numbers.Web.Transitions.ITransition';
	global.Numbers.Web.Transitions.ITransition = $Numbers_Web_Transitions_ITransition;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Transitions.IValueBounds
	var $Numbers_Web_Transitions_IValueBounds = function() {
	};
	$Numbers_Web_Transitions_IValueBounds.__typeName = 'Numbers.Web.Transitions.IValueBounds';
	global.Numbers.Web.Transitions.IValueBounds = $Numbers_Web_Transitions_IValueBounds;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Transitions.Keyframe
	var $Numbers_Web_Transitions_Keyframe = function(targetElement, targetProperty, keyframeValue, delayMilliseconds) {
		this.$1$CompletedField = null;
		this.$targetElement = null;
		this.$targetProperty = null;
		this.$keyframeValue = null;
		this.$delay = 0;
		this.$cancellationToken = 0;
		this.$targetElement = targetElement;
		this.$targetProperty = targetProperty;
		this.$keyframeValue = keyframeValue;
		this.$delay = delayMilliseconds;
	};
	$Numbers_Web_Transitions_Keyframe.__typeName = 'Numbers.Web.Transitions.Keyframe';
	global.Numbers.Web.Transitions.Keyframe = $Numbers_Web_Transitions_Keyframe;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Transitions.MultiplePropertyTransition
	var $Numbers_Web_Transitions_MultiplePropertyTransition = function(targetElement, targetProperties, bounds, timing, delayMilliseconds, continuationMode) {
		this.$1$CompletedField = null;
		this.$transition = null;
		this.$transition = new $Numbers_Web_Transitions_ParallelTransition(Enumerable.from(targetProperties).select(function(targetProperty) {
			return new $Numbers_Web_Transitions_Transition(targetElement, targetProperty, bounds, timing, delayMilliseconds, continuationMode);
		}).toArray());
		this.$transition.add_completed(ss.mkdel(this, function(sender, e) {
			this.$raiseCompleted();
		}));
	};
	$Numbers_Web_Transitions_MultiplePropertyTransition.__typeName = 'Numbers.Web.Transitions.MultiplePropertyTransition';
	global.Numbers.Web.Transitions.MultiplePropertyTransition = $Numbers_Web_Transitions_MultiplePropertyTransition;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Transitions.ParallelTransition
	var $Numbers_Web_Transitions_ParallelTransition = function(transitions) {
		this.$1$CompletedField = null;
		this.$transitions = null;
		this.$completedCount = 0;
		this.$transitions = ss.arrayClone(transitions);
		for (var $t1 = 0; $t1 < transitions.length; $t1++) {
			var transition = transitions[$t1];
			transition.add_completed(ss.mkdel(this, this.$onTransitionCompleted));
		}
	};
	$Numbers_Web_Transitions_ParallelTransition.__typeName = 'Numbers.Web.Transitions.ParallelTransition';
	global.Numbers.Web.Transitions.ParallelTransition = $Numbers_Web_Transitions_ParallelTransition;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Transitions.PixelValueBounds
	var $Numbers_Web_Transitions_PixelValueBounds = function(startValue, endValue) {
		this.$1$FormattedStartValueField = null;
		this.$1$FormattedEndValueField = null;
		this.$startValue = 0;
		this.$endValue = 0;
		this.$startValue = startValue;
		this.$endValue = endValue;
		this.set_formattedStartValue($Numbers_Web_Transitions_PixelValueBounds.$formatValue(startValue));
		this.set_formattedEndValue($Numbers_Web_Transitions_PixelValueBounds.$formatValue(endValue));
	};
	$Numbers_Web_Transitions_PixelValueBounds.__typeName = 'Numbers.Web.Transitions.PixelValueBounds';
	$Numbers_Web_Transitions_PixelValueBounds.$formatValue = function(value) {
		return ss.formatString('{0}px', value);
	};
	$Numbers_Web_Transitions_PixelValueBounds.$getValue = function(formattedValue) {
		if (ss.endsWithString(formattedValue, 'px')) {
			formattedValue = formattedValue.substr(0, formattedValue.length - 2);
		}
		return parseFloat(formattedValue);
	};
	global.Numbers.Web.Transitions.PixelValueBounds = $Numbers_Web_Transitions_PixelValueBounds;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Transitions.ScaleValueBounds
	var $Numbers_Web_Transitions_ScaleValueBounds = function(startValue, endValue) {
		this.$1$FormattedStartValueField = null;
		this.$1$FormattedEndValueField = null;
		this.$startValue = 0;
		this.$endValue = 0;
		this.$startValue = startValue;
		this.$endValue = endValue;
		this.set_formattedStartValue($Numbers_Web_Transitions_ScaleValueBounds.$formatValue(startValue));
		this.set_formattedEndValue($Numbers_Web_Transitions_ScaleValueBounds.$formatValue(endValue));
	};
	$Numbers_Web_Transitions_ScaleValueBounds.__typeName = 'Numbers.Web.Transitions.ScaleValueBounds';
	$Numbers_Web_Transitions_ScaleValueBounds.$formatValue = function(value) {
		return ss.formatString('scale({0})', value);
	};
	$Numbers_Web_Transitions_ScaleValueBounds.$getValue = function(formattedValue) {
		var match = $Numbers_Web_Transitions_ScaleValueBounds.$scaleRegex.exec(formattedValue);
		if (ss.isValue(match)) {
			formattedValue = match[1];
		}
		match = $Numbers_Web_Transitions_ScaleValueBounds.$matrixRegex.exec(formattedValue);
		if (ss.isValue(match) && ss.referenceEquals(match[1], match[2])) {
			return parseFloat(match[1]);
		}
		return parseFloat(formattedValue);
	};
	global.Numbers.Web.Transitions.ScaleValueBounds = $Numbers_Web_Transitions_ScaleValueBounds;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Transitions.SequentialTransition
	var $Numbers_Web_Transitions_SequentialTransition = function(transitions) {
		this.$1$CompletedField = null;
		this.$transitions = null;
		this.$currentIndex = 0;
		this.$transitions = ss.arrayClone(transitions);
		for (var $t1 = 0; $t1 < transitions.length; $t1++) {
			var transition = transitions[$t1];
			transition.add_completed(ss.mkdel(this, this.$onTransitionCompleted));
		}
	};
	$Numbers_Web_Transitions_SequentialTransition.__typeName = 'Numbers.Web.Transitions.SequentialTransition';
	global.Numbers.Web.Transitions.SequentialTransition = $Numbers_Web_Transitions_SequentialTransition;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Transitions.TimingCurve
	var $Numbers_Web_Transitions_TimingCurve = function(x1, y1, x2, y2, name) {
		this.$1$X1Field = 0;
		this.$1$Y1Field = 0;
		this.$1$X2Field = 0;
		this.$1$Y2Field = 0;
		this.$1$NameField = null;
		this.set_name(name);
	};
	$Numbers_Web_Transitions_TimingCurve.__typeName = 'Numbers.Web.Transitions.TimingCurve';
	$Numbers_Web_Transitions_TimingCurve.cubicBezier = function(x1, y1, x2, y2) {
		return new $Numbers_Web_Transitions_TimingCurve(x1, y1, x2, y2, null);
	};
	global.Numbers.Web.Transitions.TimingCurve = $Numbers_Web_Transitions_TimingCurve;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Transitions.Transition
	var $Numbers_Web_Transitions_Transition = function(targetElement, targetProperty, bounds, timing, delayMilliseconds, continuationMode) {
		this.$1$CompletedField = null;
		this.$targetElement = null;
		this.$targetProperty = null;
		this.$valueBounds = null;
		this.$timing = null;
		this.$delay = 0;
		this.$continuationMode = 0;
		this.$state = 0;
		this.$cancellationToken = 0;
		this.$targetElement = targetElement;
		this.$targetProperty = targetProperty;
		this.$valueBounds = bounds;
		this.$timing = timing;
		this.$delay = delayMilliseconds;
		this.$continuationMode = continuationMode;
	};
	$Numbers_Web_Transitions_Transition.__typeName = 'Numbers.Web.Transitions.Transition';
	global.Numbers.Web.Transitions.Transition = $Numbers_Web_Transitions_Transition;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Transitions.Transition.ContinuationMode
	var $Numbers_Web_Transitions_Transition$ContinuationMode = function() {
	};
	$Numbers_Web_Transitions_Transition$ContinuationMode.__typeName = 'Numbers.Web.Transitions.Transition$ContinuationMode';
	global.Numbers.Web.Transitions.Transition$ContinuationMode = $Numbers_Web_Transitions_Transition$ContinuationMode;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Transitions.TransitionTiming
	var $Numbers_Web_Transitions_TransitionTiming = function(durationMilliseconds, timing, delayMilliseconds) {
		this.$1$DurationField = 0;
		this.$1$DelayField = 0;
		this.$1$TimingField = null;
		this.set_duration(durationMilliseconds);
		this.set_delay(delayMilliseconds);
		this.set_timing(timing || $Numbers_Web_Transitions_TimingCurve.ease);
	};
	$Numbers_Web_Transitions_TransitionTiming.__typeName = 'Numbers.Web.Transitions.TransitionTiming';
	global.Numbers.Web.Transitions.TransitionTiming = $Numbers_Web_Transitions_TransitionTiming;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Transitions.ValueBoundsExtensions
	var $Numbers_Web_Transitions_ValueBoundsExtensions = function() {
	};
	$Numbers_Web_Transitions_ValueBoundsExtensions.__typeName = 'Numbers.Web.Transitions.ValueBoundsExtensions';
	$Numbers_Web_Transitions_ValueBoundsExtensions.reverse = function(valueBounds) {
		return new $Numbers_$Web_Transitions_ValueBoundsExtensions$ReversedValueBounds(valueBounds);
	};
	global.Numbers.Web.Transitions.ValueBoundsExtensions = $Numbers_Web_Transitions_ValueBoundsExtensions;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.ViewModels.CreationSource
	var $Numbers_Web_ViewModels_CreationSource = function() {
	};
	$Numbers_Web_ViewModels_CreationSource.__typeName = 'Numbers.Web.ViewModels.CreationSource';
	global.Numbers.Web.ViewModels.CreationSource = $Numbers_Web_ViewModels_CreationSource;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.ViewModels.GameViewModel
	var $Numbers_Web_ViewModels_GameViewModel = function(model, host) {
		this.$model = null;
		this.$1$SolvedField = null;
		this.$1$NumbersField = null;
		this.$1$OperatorsField = null;
		this.$selectedNumbers = null;
		this.$stepsCount = 0;
		this.$hintUsed = false;
		this.$host = null;
		this.$model = model;
		this.$host = host;
		this.$selectedNumbers = [];
		this.set_numbers(new (ss.makeGenericType($Numbers_Web_Generic_ObservableCollection$1, [$Numbers_Web_ViewModels_NumberViewModel]))());
		this.get_numbers().add_collectionChanged(ss.mkdel(this, this.$onItemsCollectionChanged));
		var $t1 = ss.getEnumerator(model.get_currentNumbers());
		try {
			while ($t1.moveNext()) {
				var number = $t1.current();
				this.get_numbers().add(new $Numbers_Web_ViewModels_NumberViewModel(number, false, 0));
			}
		}
		finally {
			$t1.dispose();
		}
		this.set_operators([new $Numbers_Web_ViewModels_OperatorViewModel('+', $Numbers_Web_Number.add), new $Numbers_Web_ViewModels_OperatorViewModel('-', $Numbers_Web_Number.subtract), new $Numbers_Web_ViewModels_OperatorViewModel('×', $Numbers_Web_Number.multiply), new $Numbers_Web_ViewModels_OperatorViewModel('÷', $Numbers_Web_Number.divide)]);
		var $t2 = ss.getEnumerator(this.get_operators());
		try {
			while ($t2.moveNext()) {
				var operatorViewModel = $t2.current();
				operatorViewModel.add_isSelectedChanged(ss.mkdel(this, this.$onOperatorIsSelectedChanged));
			}
		}
		finally {
			$t2.dispose();
		}
	};
	$Numbers_Web_ViewModels_GameViewModel.__typeName = 'Numbers.Web.ViewModels.GameViewModel';
	global.Numbers.Web.ViewModels.GameViewModel = $Numbers_Web_ViewModels_GameViewModel;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.ViewModels.NumberViewModel
	var $Numbers_Web_ViewModels_NumberViewModel = function(model, isTarget, source) {
		this.$2$ModelField = null;
		this.$2$IsTargetField = false;
		this.$2$SourceField = 0;
		$Numbers_Web_ViewModels_SelectableViewModel.call(this);
		this.set_model(model);
		this.set_isTarget(isTarget);
		this.set_source(source);
	};
	$Numbers_Web_ViewModels_NumberViewModel.__typeName = 'Numbers.Web.ViewModels.NumberViewModel';
	global.Numbers.Web.ViewModels.NumberViewModel = $Numbers_Web_ViewModels_NumberViewModel;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.ViewModels.OperatorViewModel
	var $Numbers_Web_ViewModels_OperatorViewModel = function(header, calculation) {
		this.$2$HeaderField = null;
		this.$calculation = null;
		$Numbers_Web_ViewModels_SelectableViewModel.call(this);
		this.set_header(header);
		this.$calculation = calculation;
	};
	$Numbers_Web_ViewModels_OperatorViewModel.__typeName = 'Numbers.Web.ViewModels.OperatorViewModel';
	global.Numbers.Web.ViewModels.OperatorViewModel = $Numbers_Web_ViewModels_OperatorViewModel;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.ViewModels.SelectableViewModel
	var $Numbers_Web_ViewModels_SelectableViewModel = function() {
		this.$1$IsSelectedChangedField = null;
		this.$isSelected = false;
	};
	$Numbers_Web_ViewModels_SelectableViewModel.__typeName = 'Numbers.Web.ViewModels.SelectableViewModel';
	global.Numbers.Web.ViewModels.SelectableViewModel = $Numbers_Web_ViewModels_SelectableViewModel;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Views.GameView
	var $Numbers_Web_Views_GameView = function(viewModel) {
		this.$viewModel = null;
		this.$numbersCollectionView = null;
		this.$operatorsCollectionView = null;
		this.$targetLabelAppearAnimation = null;
		this.$targetLabelDisappearAnimation = null;
		this.$solveAppearAnimation = null;
		this.$solveDisappearAnimation = null;
		this.$toolbarButtonsAppearAnimation = null;
		this.$toolbarButtonsDisappearAnimation = null;
		this.$newGameButton = null;
		this.$hintButton = null;
		this.$undoButton = null;
		this.$solved = false;
		this.$newGameRequested = false;
		$Numbers_Web_Controls_Control.call(this, ['root']);
		this.$viewModel = viewModel;
		var targetBackground1 = new $Numbers_Web_Controls_Control(['target-background1']);
		var targetBackground2 = new $Numbers_Web_Controls_Control(['target-background2']);
		var targetBackgroundOverlay1 = new $Numbers_Web_Controls_Control(['target-background-overlay1']);
		var targetBackgroundOverlay2 = new $Numbers_Web_Controls_Control(['target-background-overlay2']);
		targetBackground2.get_htmlElement().addEventListener('mousedown', ss.mkdel(this, function(e) {
			this.$newGame();
		}), false);
		this.$numbersCollectionView = new $Numbers_Web_Views_NumbersCollectionView(viewModel.get_numbers());
		this.$operatorsCollectionView = new $Numbers_Web_Views_OperatorsCollectionView(viewModel.get_operators());
		var $t1 = new $Numbers_Web_Controls_Label('target-label');
		$t1.set_text(viewModel.get_targetValue().toString());
		var targetLabel = $t1;
		var $t2 = new $Numbers_Web_Controls_ToolbarButton('new', 'ic_action_new_dark.png', ss.mkdel(this, this.$newGame));
		$t2.set_isEnabled(false);
		this.$newGameButton = $t2;
		this.$hintButton = new $Numbers_Web_Controls_ToolbarButton('hint', 'ic_action_help_dark.png', ss.mkdel(viewModel, viewModel.hint));
		this.$undoButton = new $Numbers_Web_Controls_ToolbarButton('undo', 'ic_action_undo_dark.png', ss.mkdel(viewModel, viewModel.undo));
		var $t3 = new $Numbers_Web_Controls_Control(['frame']);
		var $t4 = new $Numbers_Web_Controls_Control(['toolbar']);
		var $t5 = new $Numbers_Web_Controls_Label('header');
		$t5.set_text('Numbers');
		$t4.add($t5);
		$t4.add(this.$newGameButton);
		$t4.add(this.$hintButton);
		$t4.add(this.$undoButton);
		$t3.add($t4);
		$t3.add(targetBackground1);
		$t3.add(targetBackground2);
		$t3.add(this.$numbersCollectionView);
		$t3.add(this.$operatorsCollectionView);
		$t3.add(targetLabel);
		$t3.add(targetBackgroundOverlay1);
		$t3.add(targetBackgroundOverlay2);
		this.appendChild($t3);
		this.appendChild(new $Numbers_Web_Controls_Control(['frame-shadow']));
		viewModel.add_solved(ss.mkdel(this, this.$onSolved));
		this.$targetLabelAppearAnimation = new $Numbers_Web_Transitions_ParallelTransition([new $Numbers_Web_Transitions_Transition(targetLabel.get_htmlElement(), 'top', new $Numbers_Web_Transitions_PixelValueBounds(340, 280), new $Numbers_Web_Transitions_TransitionTiming(800, null, 0), 0, 0), new $Numbers_Web_Transitions_Transition(targetLabel.get_htmlElement(), 'opacity', new $Numbers_Web_Transitions_DoubleValueBounds(0, 1), new $Numbers_Web_Transitions_TransitionTiming(800, null, 0), 0, 0)]);
		this.$targetLabelDisappearAnimation = new $Numbers_Web_Transitions_ParallelTransition([new $Numbers_Web_Transitions_Transition(targetLabel.get_htmlElement(), 'top', new $Numbers_Web_Transitions_PixelValueBounds(280, 340), new $Numbers_Web_Transitions_TransitionTiming(800, null, 0), 0, 0), new $Numbers_Web_Transitions_Transition(targetLabel.get_htmlElement(), 'opacity', new $Numbers_Web_Transitions_DoubleValueBounds(1, 0), new $Numbers_Web_Transitions_TransitionTiming(800, null, 0), 0, 0)]);
		this.$toolbarButtonsAppearAnimation = new $Numbers_Web_Transitions_ParallelTransition([new $Numbers_Web_Transitions_Transition(this.$hintButton.get_htmlElement(), 'opacity', new $Numbers_Web_Transitions_DoubleValueBounds(0, 1), new $Numbers_Web_Transitions_TransitionTiming(800, null, 0), 0, 0), new $Numbers_Web_Transitions_Transition(this.$undoButton.get_htmlElement(), 'opacity', new $Numbers_Web_Transitions_DoubleValueBounds(0, 1), new $Numbers_Web_Transitions_TransitionTiming(800, null, 0), 0, 0)]);
		this.$toolbarButtonsDisappearAnimation = new $Numbers_Web_Transitions_ParallelTransition([new $Numbers_Web_Transitions_Transition(this.$hintButton.get_htmlElement(), 'opacity', new $Numbers_Web_Transitions_DoubleValueBounds(1, 0), new $Numbers_Web_Transitions_TransitionTiming(800, null, 0), 0, 0), new $Numbers_Web_Transitions_Transition(this.$undoButton.get_htmlElement(), 'opacity', new $Numbers_Web_Transitions_DoubleValueBounds(1, 0), new $Numbers_Web_Transitions_TransitionTiming(800, null, 0), 0, 0)]);
		this.$solveAppearAnimation = new $Numbers_Web_Transitions_ParallelTransition([new $Numbers_Web_Transitions_Keyframe(targetBackground1.get_htmlElement(), 'visibility', 'visible', 300), new $Numbers_Web_Transitions_MultiplePropertyTransition(targetBackground1.get_htmlElement(), ['transform', '-webkit-transform'], new $Numbers_Web_Transitions_ScaleValueBounds(1, 10), new $Numbers_Web_Transitions_TransitionTiming(2000, null, 0), 200, 0), new $Numbers_Web_Transitions_Transition(targetBackground1.get_htmlElement(), 'top', new $Numbers_Web_Transitions_PixelValueBounds(80, 164), new $Numbers_Web_Transitions_TransitionTiming(800, $Numbers_Web_Transitions_TimingCurve.easeOut, 0), 200, 0), new $Numbers_Web_Transitions_MultiplePropertyTransition(targetBackground1.get_htmlElement(), ['border-radius', '-webkit-border-radius'], new $Numbers_Web_Transitions_PixelValueBounds(2, 40), new $Numbers_Web_Transitions_TransitionTiming(1000, null, 0), 200, 0), new $Numbers_Web_Transitions_Keyframe(targetBackground2.get_htmlElement(), 'visibility', 'visible', 500), new $Numbers_Web_Transitions_MultiplePropertyTransition(targetBackground2.get_htmlElement(), ['transform', '-webkit-transform'], new $Numbers_Web_Transitions_ScaleValueBounds(1, 10), new $Numbers_Web_Transitions_TransitionTiming(1800, null, 0), 500, 0), new $Numbers_Web_Transitions_Transition(targetBackground2.get_htmlElement(), 'top', new $Numbers_Web_Transitions_PixelValueBounds(80, 164), new $Numbers_Web_Transitions_TransitionTiming(800, $Numbers_Web_Transitions_TimingCurve.easeOut, 0), 200, 0), new $Numbers_Web_Transitions_MultiplePropertyTransition(targetBackground2.get_htmlElement(), ['border-radius', '-webkit-border-radius'], new $Numbers_Web_Transitions_PixelValueBounds(2, 40), new $Numbers_Web_Transitions_TransitionTiming(800, null, 0), 500, 0), new $Numbers_Web_Transitions_Keyframe(this.$numbersCollectionView.get_htmlElement(), 'pointerEvents', 'none', 0), new $Numbers_Web_Transitions_Keyframe(this.$operatorsCollectionView.get_htmlElement(), 'pointerEvents', 'none', 0), new $Numbers_Web_Transitions_Transition(this.$numbersCollectionView.get_htmlElement(), 'top', new $Numbers_Web_Transitions_PixelValueBounds(80, 164), new $Numbers_Web_Transitions_TransitionTiming(800, $Numbers_Web_Transitions_TimingCurve.easeOut, 0), 200, 0), new $Numbers_Web_Transitions_Transition(this.$operatorsCollectionView.get_htmlElement(), 'top', new $Numbers_Web_Transitions_PixelValueBounds(176, 236), new $Numbers_Web_Transitions_TransitionTiming(800, null, 0), 0, 0), new $Numbers_Web_Transitions_Transition(this.$operatorsCollectionView.get_htmlElement(), 'opacity', new $Numbers_Web_Transitions_DoubleValueBounds(1, 0), new $Numbers_Web_Transitions_TransitionTiming(800, null, 0), 0, 0), new $Numbers_Web_Transitions_Transition(targetLabel.get_htmlElement(), 'top', new $Numbers_Web_Transitions_PixelValueBounds(280, 340), new $Numbers_Web_Transitions_TransitionTiming(800, null, 0), 0, 0), new $Numbers_Web_Transitions_Transition(targetLabel.get_htmlElement(), 'opacity', new $Numbers_Web_Transitions_DoubleValueBounds(1, 0), new $Numbers_Web_Transitions_TransitionTiming(800, null, 0), 0, 0), this.$toolbarButtonsDisappearAnimation]);
		this.$solveDisappearAnimation = new $Numbers_Web_Transitions_ParallelTransition([new $Numbers_Web_Transitions_Keyframe(targetBackgroundOverlay1.get_htmlElement(), 'visibility', 'visible', 0), new $Numbers_Web_Transitions_Transition(targetBackgroundOverlay1.get_htmlElement(), 'opacity', new $Numbers_Web_Transitions_DoubleValueBounds(0, 0.4), new $Numbers_Web_Transitions_TransitionTiming(1000, null, 0), 0, 0), new $Numbers_Web_Transitions_MultiplePropertyTransition(targetBackgroundOverlay1.get_htmlElement(), ['transform', '-webkit-transform'], new $Numbers_Web_Transitions_ScaleValueBounds(1, 10), new $Numbers_Web_Transitions_TransitionTiming(2000, null, 0), 0, 0), new $Numbers_Web_Transitions_Keyframe(targetBackgroundOverlay2.get_htmlElement(), 'visibility', 'visible', 200), new $Numbers_Web_Transitions_Transition(targetBackgroundOverlay2.get_htmlElement(), 'opacity', new $Numbers_Web_Transitions_DoubleValueBounds(0, 1), new $Numbers_Web_Transitions_TransitionTiming(800, null, 0), 200, 0), new $Numbers_Web_Transitions_MultiplePropertyTransition(targetBackgroundOverlay2.get_htmlElement(), ['transform', '-webkit-transform'], new $Numbers_Web_Transitions_ScaleValueBounds(0.1, 10), new $Numbers_Web_Transitions_TransitionTiming(1800, null, 0), 200, 0)]);
	};
	$Numbers_Web_Views_GameView.__typeName = 'Numbers.Web.Views.GameView';
	global.Numbers.Web.Views.GameView = $Numbers_Web_Views_GameView;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Views.NumbersCollectionView
	var $Numbers_Web_Views_NumbersCollectionView = function(viewModel) {
		this.$numbersButtons = null;
		$Numbers_Web_Controls_Control.call(this, ['numbers-panel']);
		this.$numbersButtons = new (ss.makeGenericType($Numbers_Web_Generic_ConvertedObservableCollection$2, [$Numbers_Web_ViewModels_NumberViewModel, $Numbers_Web_Controls_Button]))(viewModel, $Numbers_Web_Views_NumbersCollectionView.$createButton, null);
		this.$numbersButtons.add_collectionChanged(ss.mkdel(this, this.$onNumbersButtonsCollectionChanged));
		var $t1 = this.$numbersButtons.getEnumerator();
		try {
			while ($t1.moveNext()) {
				var button = $t1.current();
				this.$addButton(button);
			}
		}
		finally {
			$t1.dispose();
		}
		this.$updateLayout();
	};
	$Numbers_Web_Views_NumbersCollectionView.__typeName = 'Numbers.Web.Views.NumbersCollectionView';
	$Numbers_Web_Views_NumbersCollectionView.$createButton = function(numberViewModel) {
		var $t1 = new $Numbers_Web_Controls_Label('');
		$t1.set_text(numberViewModel.get_value().toString());
		var label = $t1;
		label.get_htmlElement().classList.add('button-content');
		label.get_htmlElement().classList.add($Numbers_Web_Views_NumbersCollectionView.$getLabelSizeClass(label.get_text()));
		var $t2 = new $Numbers_Web_Controls_Button(numberViewModel.get_isSelected(), ['number', $Numbers_Web_Views_NumbersCollectionView.$getLevelClass(numberViewModel)]);
		$t2.add(label);
		var button = $t2;
		button.set_isEnabled(!numberViewModel.get_isTarget());
		$Numbers_Web_StyleExtensions.getTransitionDictionary(button.get_htmlElement().style).set('left', '200ms');
		$Numbers_Web_StyleExtensions.getTransitionDictionary(button.get_shadow().get_htmlElement().style).set('left', '200ms');
		numberViewModel.add_isSelectedChanged(function(sender, e) {
			button.set_isChecked(numberViewModel.get_isSelected());
		});
		button.add_isCheckedChanged(function(sender1, e1) {
			numberViewModel.set_isSelected(button.get_isChecked());
		});
		if (numberViewModel.get_source() === 2) {
			button.startFadeInAnimation();
		}
		if (numberViewModel.get_source() === 1) {
			button.startScaleOutAnimation2();
		}
		return button;
	};
	$Numbers_Web_Views_NumbersCollectionView.$getLevelClass = function(numberViewModel) {
		return (numberViewModel.get_isTarget() ? 'target' : ss.formatString('level{0}', numberViewModel.get_level()));
	};
	$Numbers_Web_Views_NumbersCollectionView.$getLabelSizeClass = function(text) {
		if (text.length <= 4) {
			return 'medium';
		}
		if (text.length <= 5) {
			return 'small';
		}
		return 'extra-small';
	};
	global.Numbers.Web.Views.NumbersCollectionView = $Numbers_Web_Views_NumbersCollectionView;
	////////////////////////////////////////////////////////////////////////////////
	// Numbers.Web.Views.OperatorsCollectionView
	var $Numbers_Web_Views_OperatorsCollectionView = function(viewModel) {
		this.$operatorsButtons = null;
		$Numbers_Web_Controls_Control.call(this, ['operators-panel']);
		var left = 128;
		this.$operatorsButtons = Enumerable.from(viewModel).select($Numbers_Web_Views_OperatorsCollectionView.$createButton).toArray();
		var $t1 = ss.getEnumerator(this.$operatorsButtons);
		try {
			while ($t1.moveNext()) {
				var button = $t1.current();
				button.set_left(left);
				button.get_shadow().set_left(left);
				this.appendChild(button);
				this.appendChild(button.get_shadow());
				left += 88;
			}
		}
		finally {
			$t1.dispose();
		}
	};
	$Numbers_Web_Views_OperatorsCollectionView.__typeName = 'Numbers.Web.Views.OperatorsCollectionView';
	$Numbers_Web_Views_OperatorsCollectionView.$createButton = function(operatorViewModel) {
		var $t1 = new $Numbers_Web_Controls_Label('');
		$t1.set_text(operatorViewModel.get_header());
		var label = $t1;
		label.get_htmlElement().classList.add('button-content');
		var $t2 = new $Numbers_Web_Controls_Button(operatorViewModel.get_isSelected(), ['operator']);
		$t2.add(label);
		var button = $t2;
		operatorViewModel.add_isSelectedChanged(function(sender, e) {
			button.set_isChecked(operatorViewModel.get_isSelected());
		});
		button.add_isCheckedChanged(function(sender1, e1) {
			operatorViewModel.set_isSelected(button.get_isChecked());
		});
		return button;
	};
	global.Numbers.Web.Views.OperatorsCollectionView = $Numbers_Web_Views_OperatorsCollectionView;
	ss.initEnum($Numbers_$Web_Transitions_Transition$State, $asm, { $stopped: 0, $pending: 1, $running: 2 });
	ss.initInterface($Numbers_Web_Transitions_IValueBounds, $asm, { get_formattedStartValue: null, get_formattedEndValue: null, getProgress: null });
	ss.initClass($Numbers_$Web_Transitions_ValueBoundsExtensions$ReversedValueBounds, $asm, {
		get_formattedStartValue: function() {
			return this.$source.get_formattedEndValue();
		},
		get_formattedEndValue: function() {
			return this.$source.get_formattedStartValue();
		},
		getProgress: function(formattedValue) {
			return 1 - this.$source.getProgress(formattedValue);
		}
	}, null, [$Numbers_Web_Transitions_IValueBounds]);
	ss.initInterface($Numbers_Web_IGameHost, $asm, { newGame: null, restorePreviousGame: null });
	ss.initClass($Numbers_Web_Application, $asm, {
		get_$level: function() {
			return this.$level;
		},
		set_$level: function(value) {
			this.$level = value;
			$Numbers_Web_ConfigurationExtensions.setValue(this.$configuration, $Numbers_Web_Application.$levelConfigurationKey, value.toString());
		},
		get_$game: function() {
			return this.$game;
		},
		set_$game: function(value) {
			if (ss.referenceEquals(this.$game, value)) {
				return;
			}
			this.$game = value;
			this.$onGameChanged();
		},
		run: function() {
			this.$configuration = new $Numbers_Web_Configuration();
			var storedLevel = {};
			this.$level = (ss.Int32.tryParse(this.$configuration.getValue($Numbers_Web_Application.$levelConfigurationKey), storedLevel) ? storedLevel.$ : $Numbers_Web_Application.$defaultLevel);
			window.addEventListener('hashchange', ss.mkdel(this, function(e) {
				this.$onHashChanged();
			}));
			if (!ss.isNullOrEmptyString(window.location.hash)) {
				this.set_$game($Numbers_Web_GameFactory.createFromHash(ss.trimStartString(window.location.hash, [35])));
				this.$customGame = true;
			}
			else {
				this.set_$game($Numbers_Web_GameFactory.createFromSolutionRange(this.get_$level() - $Numbers_Web_Application.$levelMargins, this.get_$level() + $Numbers_Web_Application.$levelMargins));
				this.$customGame = false;
			}
			window.addEventListener('resize', ss.mkdel(this, function(e1) {
				this.$updateLayout();
			}));
		},
		newGame: function(levelChange) {
			console.log('new game');
			if (!this.$customGame) {
				if (levelChange === 0) {
					this.set_$level(ss.Int32.trunc(Math.min(this.get_$level() * 1.2, 100)));
				}
				if (levelChange === 2) {
					this.set_$level(ss.Int32.trunc(Math.max(this.get_$level() * 0.8, 5)));
				}
			}
			this.set_$game($Numbers_Web_GameFactory.createFromSolutionRange(this.get_$level() - $Numbers_Web_Application.$levelMargins, this.get_$level() + $Numbers_Web_Application.$levelMargins));
			this.$customGame = false;
		},
		restorePreviousGame: function() {
			//
		},
		$onHashChanged: function() {
			if (!ss.referenceEquals(this.get_$game().toString(), ss.trimStartString(window.location.hash, [35]))) {
				this.set_$game($Numbers_Web_GameFactory.createFromHash(ss.trimStartString(window.location.hash, [35])));
				this.$customGame = true;
			}
		},
		$onGameChanged: function() {
			window.history.replaceState(null, document.title, ss.formatString('#{0}', this.get_$game()));
			var gameViewModel = new $Numbers_Web_ViewModels_GameViewModel(this.get_$game(), this);
			this.$gameView = new $Numbers_Web_Views_GameView(gameViewModel);
			this.$updateLayout();
			while (document.body.children.length > 0) {
				document.body.removeChild(document.body.lastChild);
			}
			document.body.appendChild(this.$gameView.get_htmlElement());
			this.$gameView.run();
		},
		$updateLayout: function() {
			if (ss.isNullOrUndefined(this.$gameView)) {
				return;
			}
			this.$gameView.set_left(Math.max(0, ss.Int32.div(window.innerWidth - $Numbers_Web_Views_GameView.width, 2)));
			this.$gameView.set_top(Math.max(0, ss.Int32.div(window.innerHeight - $Numbers_Web_Views_GameView.height, 2)));
			document.body.style.width = ss.formatString('{0}px', window.innerWidth);
			document.body.style.height = ss.formatString('{0}px', window.innerHeight);
		}
	}, null, [$Numbers_Web_IGameHost]);
	ss.initInterface($Numbers_Web_IConfiguration, $asm, { getValue: null, setValue: null });
	ss.initClass($Numbers_Web_Configuration, $asm, {
		getValue: function(key) {
			var $t1 = document.cookie.split(';');
			for (var $t2 = 0; $t2 < $t1.length; $t2++) {
				var keyValue = $t1[$t2];
				var index = keyValue.indexOf('=');
				var currentKey = keyValue.substr(0, index).trim();
				if (ss.referenceEquals(currentKey, key)) {
					return keyValue.substring(index + 1);
				}
			}
			return '';
		},
		setValue: function(key, value, expiration) {
			document.cookie = ss.formatString('{0}={1}; expires={2}', key, value, ss.formatDate(expiration, $Numbers_Web_Configuration.$gmtTimeFormat));
		}
	}, null, [$Numbers_Web_IConfiguration]);
	ss.initClass($Numbers_Web_ConfigurationExtensions, $asm, {});
	ss.initClass($Numbers_Web_Game, $asm, {
		get_initialValues: function() {
			return this.$1$InitialValuesField;
		},
		set_initialValues: function(value) {
			this.$1$InitialValuesField = value;
		},
		get_currentNumbers: function() {
			return this.$1$CurrentNumbersField;
		},
		set_currentNumbers: function(value) {
			this.$1$CurrentNumbersField = value;
		},
		get_targetValue: function() {
			return this.$1$TargetValueField;
		},
		set_targetValue: function(value) {
			this.$1$TargetValueField = value;
		},
		get_isSolved: function() {
			return Enumerable.from(this.get_currentNumbers()).count() === 1 && Enumerable.from(this.get_currentNumbers()).first().get_value() === this.get_targetValue();
		},
		toString: function() {
			var valuesString = Enumerable.from(this.get_initialValues()).select(function(value) {
				return value.toString();
			}).aggregate(function(value1, value2) {
				return ss.formatString('{0}-{1}', value1, value2);
			});
			return ss.formatString('{0}-{1}', valuesString, this.get_targetValue());
		},
		push: function(result) {
			if (!Enumerable.from(this.get_currentNumbers()).contains(result.get_operand1()) || !Enumerable.from(this.get_currentNumbers()).contains(result.get_operand2())) {
				throw new ss.Exception('Result was not created with current numbers');
			}
			this.$stack.push(result);
			this.set_currentNumbers(Enumerable.from(this.get_currentNumbers()).where(function(number) {
				return $Numbers_Web_Number.op_Inequality(number, result.get_operand1()) && $Numbers_Web_Number.op_Inequality(number, result.get_operand2());
			}).concat([result]).toArray());
		},
		pop: function() {
			if (this.$stack.length === 0) {
				return null;
			}
			var result = this.$stack.pop();
			this.set_currentNumbers(Enumerable.from(this.get_currentNumbers()).where(function(number) {
				return $Numbers_Web_Number.op_Inequality(number, result);
			}).concat([result.get_operand1(), result.get_operand2()]).toArray());
			return result;
		},
		hint: function() {
			var numbers = Enumerable.from(this.get_currentNumbers()).toArray();
			numbers.sort(function(number1, number2) {
				return number1.compareTo(number2);
			});
			var solution = $Numbers_Web_Solver.findSolution(numbers, this.get_targetValue());
			if ($Numbers_Web_Number.op_Inequality(solution, null)) {
				return $Numbers_Web_Solver.findInitialOperation(solution, numbers);
			}
			return null;
		}
	});
	ss.initClass($Numbers_Web_GameFactory, $asm, {});
	ss.initEnum($Numbers_Web_LevelChange, $asm, { easier: 0, same: 1, harder: 2 });
	ss.initClass($Numbers_Web_Number, $asm, {
		get_value: function() {
			return this.$1$ValueField;
		},
		set_value: function(value) {
			this.$1$ValueField = value;
		},
		get_level: function() {
			return this.$1$LevelField;
		},
		set_level: function(value) {
			this.$1$LevelField = value;
		},
		get_operand1: function() {
			return this.$1$Operand1Field;
		},
		set_operand1: function(value) {
			this.$1$Operand1Field = value;
		},
		get_operand2: function() {
			return this.$1$Operand2Field;
		},
		set_operand2: function(value) {
			this.$1$Operand2Field = value;
		},
		get_operator: function() {
			return this.$1$OperatorField;
		},
		set_operator: function(value) {
			this.$1$OperatorField = value;
		},
		toString: function() {
			return this.toString$1(false, true);
		},
		toString$1: function(includeValue, reduceParentheses) {
			if (this.get_operator() === 0) {
				return this.get_value().toString();
			}
			var stringBuilder = new ss.StringBuilder();
			if (includeValue) {
				stringBuilder.append(this.get_value());
				stringBuilder.append('=');
			}
			if (this.get_operand1().get_operator() === 0 || !includeValue && reduceParentheses && (this.get_operator() === 1 || this.get_operator() === 2 || (this.get_operator() === 3 || this.get_operator() === 4) && (this.get_operand1().get_operator() === 3 || this.get_operand1().get_operator() === 4))) {
				stringBuilder.append(this.get_operand1().toString$1(includeValue, reduceParentheses));
			}
			else {
				stringBuilder.append(ss.formatString('({0})', this.get_operand1().toString$1(includeValue, reduceParentheses)));
			}
			stringBuilder.append($Numbers_Web_Number.$getOperatorString(this.get_operator()));
			if (this.get_operand2().get_operator() === 0 || !includeValue && reduceParentheses && (this.get_operator() === 1 || (this.get_operator() === 2 || this.get_operator() === 3 || this.get_operator() === 4) && (this.get_operand2().get_operator() === 3 || this.get_operand2().get_operator() === 4))) {
				stringBuilder.append(this.get_operand2().toString$1(includeValue, reduceParentheses));
			}
			else {
				stringBuilder.append(ss.formatString('({0})', this.get_operand2().toString$1(includeValue, reduceParentheses)));
			}
			return stringBuilder.toString();
		},
		equals: function(obj) {
			if (ss.isNullOrUndefined(obj) || !ss.referenceEquals(ss.getInstanceType(obj), ss.getInstanceType(this))) {
				return false;
			}
			var number = ss.safeCast(obj, $Numbers_Web_Number);
			return ss.equalsT(this.get_level(), number.get_level()) && ss.equalsT(this.get_value(), number.get_value()) && ss.equals(this.get_operator(), number.get_operator()) && (ss.referenceEquals(this.get_operand1(), null) && ss.referenceEquals(number.get_operand1(), null) || !ss.referenceEquals(this.get_operand1(), null) && this.get_operand1().equals(number.get_operand1())) && (ss.referenceEquals(this.get_operand2(), null) && ss.referenceEquals(number.get_operand2(), null) || !ss.referenceEquals(this.get_operand2(), null) && this.get_operand2().equals(number.get_operand2()));
		},
		getHashCode: function() {
			return this.get_level() ^ this.get_value() ^ ($Numbers_Web_Number.op_Inequality(this.get_operand1(), null) ? this.get_operand1().getHashCode() : 0) ^ ($Numbers_Web_Number.op_Inequality(this.get_operand2(), null) ? this.get_operand2().getHashCode() : 0);
		},
		compareTo: function(other) {
			var result = ss.compare(this.get_value(), other.get_value());
			if (result !== 0 || this.get_operator() === 0 && other.get_operator() === 0) {
				return result;
			}
			if (this.get_operator() === 0) {
				return -1;
			}
			if (other.get_operator() === 0) {
				return 1;
			}
			result = this.get_operand1().compareTo(other.get_operand1());
			if (result !== 0) {
				return result;
			}
			return this.get_operand2().compareTo(other.get_operand2());
		}
	}, null, [ss.IComparable]);
	ss.initEnum($Numbers_Web_Operator, $asm, { create: 0, add: 1, subtract: 2, multiply: 3, divide: 4 });
	ss.initClass($Numbers_Web_Solver, $asm, {});
	ss.initClass($Numbers_Web_StyleExtensions, $asm, {});
	ss.initClass($Numbers_Web_TokenDictionary, $asm, {
		set: function(key, value) {
			var dictionary = $Numbers_Web_TokenDictionary.$createDictionary(this.$getRawList());
			dictionary.set_item(key, value);
			this.$setRawList($Numbers_Web_TokenDictionary.$createRawList(dictionary));
		},
		contains: function(key) {
			var dictionary = $Numbers_Web_TokenDictionary.$createDictionary(this.$getRawList());
			return dictionary.containsKey(key);
		},
		clear: function(key) {
			var dictionary = $Numbers_Web_TokenDictionary.$createDictionary(this.$getRawList());
			dictionary.remove(key);
			this.$setRawList($Numbers_Web_TokenDictionary.$createRawList(dictionary));
		}
	});
	ss.initClass($Numbers_Web_Controls_Control, $asm, {
		get_htmlElement: function() {
			return this.$1$HtmlElementField;
		},
		set_htmlElement: function(value) {
			this.$1$HtmlElementField = value;
		},
		get_left: function() {
			return this.$left;
		},
		set_left: function(value) {
			this.$left = value;
			this.get_htmlElement().style.left = ss.formatString('{0}px', this.$left);
		},
		get_top: function() {
			return this.$top;
		},
		set_top: function(value) {
			this.$top = value;
			this.get_htmlElement().style.top = ss.formatString('{0}px', this.$top);
		},
		get_children: function() {
			return this.$children;
		},
		appendChild: function(child) {
			ss.add(this.$children, child);
			this.get_htmlElement().appendChild(child.get_htmlElement());
		},
		insertChild: function(index, child) {
			ss.insert(this.$children, index, child);
			if (index < this.get_htmlElement().children.length) {
				this.get_htmlElement().insertBefore(child.get_htmlElement(), this.get_htmlElement().children[index]);
			}
			else {
				this.get_htmlElement().appendChild(child.get_htmlElement());
			}
		},
		removeChild: function(child) {
			ss.remove(this.$children, child);
			this.get_htmlElement().removeChild(child.get_htmlElement());
		},
		childIndex: function(child) {
			return ss.indexOf(this.$children, child);
		},
		childAt: function(index) {
			return Enumerable.from(this.$children).where(ss.mkdel(this, function(child) {
				return ss.referenceEquals(child.get_htmlElement(), this.get_htmlElement().children[index]);
			})).firstOrDefault(null, ss.getDefaultValue($Numbers_Web_Controls_Control));
		},
		add: function(item) {
			this.appendChild(ss.cast(item, $Numbers_Web_Controls_Control));
		},
		getEnumerator: function() {
			return null;
		}
	}, null, [ss.IEnumerable]);
	ss.initClass($Numbers_Web_Controls_Button, $asm, {
		get_isEnabled: function() {
			return this.$2$IsEnabledField;
		},
		set_isEnabled: function(value) {
			this.$2$IsEnabledField = value;
		},
		add_isCheckedChanged: function(value) {
			this.$2$IsCheckedChangedField = ss.delegateCombine(this.$2$IsCheckedChangedField, value);
		},
		remove_isCheckedChanged: function(value) {
			this.$2$IsCheckedChangedField = ss.delegateRemove(this.$2$IsCheckedChangedField, value);
		},
		get_isChecked: function() {
			return this.$isChecked;
		},
		set_isChecked: function(value) {
			if (this.$isChecked !== value) {
				this.$isChecked = value;
				this.$onIsCheckChanged();
				window.setTimeout(ss.mkdel(this, this.$raiseIsCheckedChanged), $Numbers_Web_Controls_Button.$checkAnimationDurationMilliseconds);
			}
		},
		get_shadow: function() {
			return this.$2$ShadowField;
		},
		set_shadow: function(value) {
			this.$2$ShadowField = value;
		},
		startScaleInAnimation: function() {
			var animation = new $Numbers_Web_Transitions_ParallelTransition([new $Numbers_Web_Transitions_MultiplePropertyTransition(this.get_htmlElement(), ['transform', '-webkit-transform'], new $Numbers_Web_Transitions_ScaleValueBounds(0, 1), new $Numbers_Web_Transitions_TransitionTiming(400, null, 0), 0, 0), new $Numbers_Web_Transitions_Transition(this.get_htmlElement(), 'opacity', new $Numbers_Web_Transitions_DoubleValueBounds(0, 1), new $Numbers_Web_Transitions_TransitionTiming(400, null, 0), 0, 0)]);
			animation.start();
		},
		startScaleOutAnimation: function() {
			var animation = new $Numbers_Web_Transitions_ParallelTransition([new $Numbers_Web_Transitions_Keyframe(this.get_shadow().get_htmlElement(), 'visibility', 'hidden', 0), new $Numbers_Web_Transitions_MultiplePropertyTransition(this.get_htmlElement(), ['transform', '-webkit-transform'], new $Numbers_Web_Transitions_ScaleValueBounds(1, 0), new $Numbers_Web_Transitions_TransitionTiming(400, null, 0), 0, 0), new $Numbers_Web_Transitions_Transition(this.get_htmlElement(), 'opacity', new $Numbers_Web_Transitions_DoubleValueBounds(1, 0), new $Numbers_Web_Transitions_TransitionTiming(400, null, 0), 0, 0)]);
			animation.start();
		},
		startScaleOutAnimation2: function() {
			var transformTransition = new $Numbers_Web_Transitions_MultiplePropertyTransition(this.get_htmlElement(), ['transform', '-webkit-transform'], new $Numbers_Web_Transitions_ScaleValueBounds(1.4, (this.get_isChecked() ? 1.06 : 1)), new $Numbers_Web_Transitions_TransitionTiming(400, null, 0), 0, 0);
			var opacityTransition = new $Numbers_Web_Transitions_Transition(this.get_htmlElement(), 'opacity', new $Numbers_Web_Transitions_DoubleValueBounds(0, 1), new $Numbers_Web_Transitions_TransitionTiming(400, null, 0), 0, 0);
			var shadowTransformTransition = new $Numbers_Web_Transitions_MultiplePropertyTransition(this.get_shadow().get_htmlElement(), ['transform', '-webkit-transform'], new $Numbers_Web_Transitions_ScaleValueBounds(1.4, (this.get_isChecked() ? 1.06 : 1)), new $Numbers_Web_Transitions_TransitionTiming(400, null, 0), 0, 0);
			var shadowOpacityTransition = new $Numbers_Web_Transitions_Transition(this.get_shadow().get_htmlElement(), 'opacity', new $Numbers_Web_Transitions_DoubleValueBounds(0, 1), new $Numbers_Web_Transitions_TransitionTiming(400, null, 0), 0, 0);
			var scaleOutAnimation = (this.get_isChecked() ? new $Numbers_Web_Transitions_ParallelTransition([transformTransition, opacityTransition, shadowTransformTransition, shadowOpacityTransition]) : new $Numbers_Web_Transitions_ParallelTransition([transformTransition, opacityTransition]));
			scaleOutAnimation.start();
		},
		startFadeInAnimation: function() {
			var animation = new $Numbers_Web_Transitions_Transition(this.get_htmlElement(), 'opacity', new $Numbers_Web_Transitions_DoubleValueBounds(0, 1), new $Numbers_Web_Transitions_TransitionTiming(1400, null, 0), 0, 0);
			animation.start();
		},
		$onMouseDown: function(e) {
			if (!this.get_isEnabled()) {
				return;
			}
			this.set_isChecked(!this.get_isChecked());
			this.$overlayAnimation.start();
			this.$overlay.get_htmlElement().style['transformOrigin'] = ss.formatString('{0}px {1}px', ss.safeCast(e, MouseEvent).clientX, ss.safeCast(e, MouseEvent).clientY);
		},
		$onIsCheckChanged: function() {
			if (this.get_isChecked()) {
				this.$checkedAnimation.start();
			}
			else {
				this.$uncheckedAnimation.start();
			}
			window.setTimeout(ss.mkdel(this, function() {
				this.get_htmlElement().setAttribute('data-is-checked', this.get_isChecked().toString());
			}), 200);
		},
		$raiseIsCheckedChanged: function() {
			if (!ss.staticEquals(this.$2$IsCheckedChangedField, null)) {
				this.$2$IsCheckedChangedField(this, ss.EventArgs.Empty);
			}
		}
	}, $Numbers_Web_Controls_Control, [ss.IEnumerable]);
	ss.initClass($Numbers_Web_Controls_Label, $asm, {
		get_text: function() {
			return this.get_htmlElement().textContent;
		},
		set_text: function(value) {
			this.get_htmlElement().textContent = value;
		}
	}, $Numbers_Web_Controls_Control, [ss.IEnumerable]);
	ss.initClass($Numbers_Web_Controls_ToolbarButton, $asm, {
		get_isEnabled: function() {
			return this.$2$IsEnabledField;
		},
		set_isEnabled: function(value) {
			this.$2$IsEnabledField = value;
		}
	}, $Numbers_Web_Controls_Control, [ss.IEnumerable]);
	ss.initInterface($Numbers_Web_Generic_INotifyCollectionChanged, $asm, { add_collectionChanged: null, remove_collectionChanged: null });
	ss.initInterface($Numbers_Web_Generic_INotifyPropertyChanged, $asm, { add_propertyChanged: null, remove_propertyChanged: null });
	ss.initEnum($Numbers_Web_Generic_NotifyCollectionChangedAction, $asm, { add: 0, remove: 1, reset: 2 });
	ss.initClass($Numbers_Web_Generic_NotifyCollectionChangedEventArgs, $asm, {
		get_action: function() {
			return this.$2$ActionField;
		},
		set_action: function(value) {
			this.$2$ActionField = value;
		},
		get_item: function() {
			return this.$2$ItemField;
		},
		set_item: function(value) {
			this.$2$ItemField = value;
		},
		get_index: function() {
			return this.$2$IndexField;
		},
		set_index: function(value) {
			this.$2$IndexField = value;
		}
	}, ss.EventArgs);
	ss.initClass($Numbers_Web_Generic_PropertyChangedEventArgs, $asm, {
		get_propertyName: function() {
			return this.$2$PropertyNameField;
		},
		set_propertyName: function(value) {
			this.$2$PropertyNameField = value;
		}
	}, ss.EventArgs);
	ss.initClass($Numbers_Web_Transitions_DoubleValueBounds, $asm, {
		get_formattedStartValue: function() {
			return this.$1$FormattedStartValueField;
		},
		set_formattedStartValue: function(value) {
			this.$1$FormattedStartValueField = value;
		},
		get_formattedEndValue: function() {
			return this.$1$FormattedEndValueField;
		},
		set_formattedEndValue: function(value) {
			this.$1$FormattedEndValueField = value;
		},
		getProgress: function(formattedValue) {
			var value = parseFloat(formattedValue);
			return (value - this.$startValue) / (this.$endValue - this.$startValue);
		}
	}, null, [$Numbers_Web_Transitions_IValueBounds]);
	ss.initInterface($Numbers_Web_Transitions_ITransition, $asm, { add_completed: null, remove_completed: null, start: null, stop: null });
	ss.initClass($Numbers_Web_Transitions_Keyframe, $asm, {
		add_completed: function(value) {
			this.$1$CompletedField = ss.delegateCombine(this.$1$CompletedField, value);
		},
		remove_completed: function(value) {
			this.$1$CompletedField = ss.delegateRemove(this.$1$CompletedField, value);
		},
		start: function() {
			if (this.$delay === 0) {
				this.$setKeyframeValue();
			}
			else {
				this.$cancellationToken = window.setTimeout(ss.mkdel(this, this.$setKeyframeValue), this.$delay);
			}
		},
		stop: function() {
			window.clearTimeout(this.$cancellationToken);
		},
		$setKeyframeValue: function() {
			this.$targetElement.style[this.$targetProperty] = this.$keyframeValue;
			this.$raiseCompleted();
		},
		$raiseCompleted: function() {
			if (!ss.staticEquals(this.$1$CompletedField, null)) {
				this.$1$CompletedField(this, ss.EventArgs.Empty);
			}
		}
	}, null, [$Numbers_Web_Transitions_ITransition]);
	ss.initClass($Numbers_Web_Transitions_MultiplePropertyTransition, $asm, {
		add_completed: function(value) {
			this.$1$CompletedField = ss.delegateCombine(this.$1$CompletedField, value);
		},
		remove_completed: function(value) {
			this.$1$CompletedField = ss.delegateRemove(this.$1$CompletedField, value);
		},
		start: function() {
			this.$transition.start();
		},
		stop: function() {
			this.$transition.stop();
		},
		$raiseCompleted: function() {
			if (!ss.staticEquals(this.$1$CompletedField, null)) {
				this.$1$CompletedField(this, ss.EventArgs.Empty);
			}
		}
	}, null, [$Numbers_Web_Transitions_ITransition]);
	ss.initClass($Numbers_Web_Transitions_ParallelTransition, $asm, {
		add_completed: function(value) {
			this.$1$CompletedField = ss.delegateCombine(this.$1$CompletedField, value);
		},
		remove_completed: function(value) {
			this.$1$CompletedField = ss.delegateRemove(this.$1$CompletedField, value);
		},
		start: function() {
			this.$completedCount = 0;
			for (var $t1 = 0; $t1 < this.$transitions.length; $t1++) {
				var transition = this.$transitions[$t1];
				transition.start();
			}
		},
		stop: function() {
			for (var $t1 = 0; $t1 < this.$transitions.length; $t1++) {
				var transition = this.$transitions[$t1];
				transition.stop();
			}
		},
		$onTransitionCompleted: function(sender, e) {
			this.$completedCount++;
			if (this.$completedCount === this.$transitions.length) {
				this.$raiseCompleted();
			}
		},
		$raiseCompleted: function() {
			if (!ss.staticEquals(this.$1$CompletedField, null)) {
				this.$1$CompletedField(this, ss.EventArgs.Empty);
			}
		}
	}, null, [$Numbers_Web_Transitions_ITransition]);
	ss.initClass($Numbers_Web_Transitions_PixelValueBounds, $asm, {
		get_formattedStartValue: function() {
			return this.$1$FormattedStartValueField;
		},
		set_formattedStartValue: function(value) {
			this.$1$FormattedStartValueField = value;
		},
		get_formattedEndValue: function() {
			return this.$1$FormattedEndValueField;
		},
		set_formattedEndValue: function(value) {
			this.$1$FormattedEndValueField = value;
		},
		getProgress: function(formattedValue) {
			var value = $Numbers_Web_Transitions_PixelValueBounds.$getValue(formattedValue);
			return (value - this.$startValue) / (this.$endValue - this.$startValue);
		}
	}, null, [$Numbers_Web_Transitions_IValueBounds]);
	ss.initClass($Numbers_Web_Transitions_ScaleValueBounds, $asm, {
		get_formattedStartValue: function() {
			return this.$1$FormattedStartValueField;
		},
		set_formattedStartValue: function(value) {
			this.$1$FormattedStartValueField = value;
		},
		get_formattedEndValue: function() {
			return this.$1$FormattedEndValueField;
		},
		set_formattedEndValue: function(value) {
			this.$1$FormattedEndValueField = value;
		},
		getProgress: function(formattedValue) {
			var value = $Numbers_Web_Transitions_ScaleValueBounds.$getValue(formattedValue);
			return (value - this.$startValue) / (this.$endValue - this.$startValue);
		}
	}, null, [$Numbers_Web_Transitions_IValueBounds]);
	ss.initClass($Numbers_Web_Transitions_SequentialTransition, $asm, {
		add_completed: function(value) {
			this.$1$CompletedField = ss.delegateCombine(this.$1$CompletedField, value);
		},
		remove_completed: function(value) {
			this.$1$CompletedField = ss.delegateRemove(this.$1$CompletedField, value);
		},
		start: function() {
			this.$currentIndex = 0;
			if (this.$currentIndex < this.$transitions.length) {
				this.$transitions[this.$currentIndex].start();
			}
		},
		stop: function() {
			if (this.$currentIndex < this.$transitions.length) {
				this.$transitions[this.$currentIndex].stop();
			}
		},
		$onTransitionCompleted: function(sender, e) {
			this.$currentIndex++;
			if (this.$currentIndex < this.$transitions.length) {
				this.$transitions[this.$currentIndex].start();
			}
			else {
				this.$raiseCompleted();
			}
		},
		$raiseCompleted: function() {
			if (!ss.staticEquals(this.$1$CompletedField, null)) {
				this.$1$CompletedField(this, ss.EventArgs.Empty);
			}
		}
	}, null, [$Numbers_Web_Transitions_ITransition]);
	ss.initClass($Numbers_Web_Transitions_TimingCurve, $asm, {
		get_x1: function() {
			return this.$1$X1Field;
		},
		set_x1: function(value) {
			this.$1$X1Field = value;
		},
		get_y1: function() {
			return this.$1$Y1Field;
		},
		set_y1: function(value) {
			this.$1$Y1Field = value;
		},
		get_x2: function() {
			return this.$1$X2Field;
		},
		set_x2: function(value) {
			this.$1$X2Field = value;
		},
		get_y2: function() {
			return this.$1$Y2Field;
		},
		set_y2: function(value) {
			this.$1$Y2Field = value;
		},
		get_name: function() {
			return this.$1$NameField;
		},
		set_name: function(value) {
			this.$1$NameField = value;
		},
		toString: function() {
			return (!ss.isNullOrEmptyString(this.get_name()) ? this.get_name() : ss.formatString('cubic-bezier({0}, {1}, {2}, {3})', this.get_x1(), this.get_y1(), this.get_x2(), this.get_y2()));
		},
		getProgress: function(timing) {
			var resultTiming = {};
			var resultProgress = {};
			this.$findCurvePoint(function(currentTiming, currentProgress) {
				return ss.compare(currentTiming, timing);
			}, resultTiming, resultProgress);
			return resultProgress.$;
		},
		getTiming: function(progress) {
			var resultTiming = {};
			var resultProgress = {};
			this.$findCurvePoint(function(currentTiming, currentProgress) {
				return ss.compare(currentProgress, progress);
			}, resultTiming, resultProgress);
			return resultProgress.$;
		},
		$findCurvePoint: function(comparer, x, y) {
			var t = 0.5;
			var step = 0.5;
			x.$ = 0;
			y.$ = 0;
			for (var i = 0; i < 10; i++) {
				this.$getCurvePoint(t, x, y);
				if (comparer(x.$, y.$) < 0) {
					t += step;
				}
				else if (comparer(x.$, y.$) > 0) {
					t -= step;
				}
				step = step / 2;
			}
		},
		$getCurvePoint: function(t, x, y) {
			x.$ = (3 * this.get_x1() - 3 * this.get_x2() + 1) * t * t * t + (-6 * this.get_x1() + 3 * this.get_x2()) * t * t + 3 * this.get_x1() * t;
			y.$ = (3 * this.get_y1() - 3 * this.get_y2() + 1) * t * t * t + (-6 * this.get_y1() + 3 * this.get_y2()) * t * t + 3 * this.get_y1() * t;
		}
	});
	ss.initClass($Numbers_Web_Transitions_Transition, $asm, {
		add_completed: function(value) {
			this.$1$CompletedField = ss.delegateCombine(this.$1$CompletedField, value);
		},
		remove_completed: function(value) {
			this.$1$CompletedField = ss.delegateRemove(this.$1$CompletedField, value);
		},
		start: function() {
			if (this.$state !== 0) {
				this.stop();
			}
			this.$state = 1;
			this.$cancellationToken = window.setTimeout(ss.mkdel(this, function() {
				if (this.$state !== 1) {
					return;
				}
				this.$state = 2;
				var currentValue = this.$valueBounds.get_formattedStartValue();
				var currentTiming = this.$timing;
				if (this.$continuationMode === 1 || this.$continuationMode === 2) {
					currentValue = window.getComputedStyle(this.$targetElement).getPropertyValue(this.$targetProperty);
				}
				if (this.$continuationMode === 2) {
					var currentProgress = this.$valueBounds.getProgress(currentValue);
					var currentProgressTiming = this.$timing.get_timing().getTiming(currentProgress);
					currentTiming = this.$timing.addDuration(ss.Int32.trunc(-currentProgressTiming * this.$timing.get_duration()));
					// also, a truncated timing curve is needed here;
				}
				$Numbers_Web_StyleExtensions.getTransitionDictionary(this.$targetElement.style).clear(this.$targetProperty);
				this.$targetElement.style[this.$targetProperty] = currentValue;
				window.requestAnimationFrame(ss.mkdel(this, function() {
					window.requestAnimationFrame(ss.mkdel(this, function() {
						if (this.$state === 2) {
							$Numbers_Web_StyleExtensions.getTransitionDictionary(this.$targetElement.style).set(this.$targetProperty, currentTiming.toString());
							this.$targetElement.style[this.$targetProperty] = this.$valueBounds.get_formattedEndValue();
						}
					}));
				}));
				window.setTimeout(ss.mkdel(this, function() {
					if (this.$state === 2) {
						this.$raiseCompleted();
					}
				}), currentTiming.get_delay() + currentTiming.get_duration());
			}), this.$delay);
		},
		stop: function() {
			if (this.$state === 0) {
				return;
			}
			if (this.$state === 1) {
				window.clearTimeout(this.$cancellationToken);
			}
			else if (this.$state === 2) {
				var currentValue = window.getComputedStyle(this.$targetElement).getPropertyValue(this.$targetProperty);
				$Numbers_Web_StyleExtensions.getTransitionDictionary(this.$targetElement.style).clear(this.$targetProperty);
				this.$targetElement.style[this.$targetProperty] = currentValue;
			}
			else {
				throw new ss.Exception('Unsupported animation state');
			}
			this.$state = 0;
		},
		$raiseCompleted: function() {
			if (!ss.staticEquals(this.$1$CompletedField, null)) {
				this.$1$CompletedField(this, ss.EventArgs.Empty);
			}
		}
	}, null, [$Numbers_Web_Transitions_ITransition]);
	ss.initEnum($Numbers_Web_Transitions_Transition$ContinuationMode, $asm, { restart: 0, continueValue: 1, continueValueAndTime: 2 });
	ss.initClass($Numbers_Web_Transitions_TransitionTiming, $asm, {
		get_duration: function() {
			return this.$1$DurationField;
		},
		set_duration: function(value) {
			this.$1$DurationField = value;
		},
		get_delay: function() {
			return this.$1$DelayField;
		},
		set_delay: function(value) {
			this.$1$DelayField = value;
		},
		get_timing: function() {
			return this.$1$TimingField;
		},
		set_timing: function(value) {
			this.$1$TimingField = value;
		},
		toString: function() {
			return ((this.get_delay() === 0) ? ss.formatString('{0}ms {1}', this.get_duration(), this.get_timing()) : ss.formatString('{0}ms {1} {2}ms', this.get_duration(), this.get_timing(), this.get_delay()));
		},
		addDuration: function(durationMilliseconds) {
			return new $Numbers_Web_Transitions_TransitionTiming(this.get_duration() + durationMilliseconds, this.get_timing(), this.get_delay());
		},
		addDelay: function(delayMilliseconds) {
			return new $Numbers_Web_Transitions_TransitionTiming(this.get_duration(), this.get_timing(), this.get_delay() + delayMilliseconds);
		}
	});
	ss.initClass($Numbers_Web_Transitions_ValueBoundsExtensions, $asm, {});
	ss.initEnum($Numbers_Web_ViewModels_CreationSource, $asm, { initial: 0, result: 1, undo: 2 });
	ss.initClass($Numbers_Web_ViewModels_GameViewModel, $asm, {
		add_solved: function(value) {
			this.$1$SolvedField = ss.delegateCombine(this.$1$SolvedField, value);
		},
		remove_solved: function(value) {
			this.$1$SolvedField = ss.delegateRemove(this.$1$SolvedField, value);
		},
		get_numbers: function() {
			return this.$1$NumbersField;
		},
		set_numbers: function(value) {
			this.$1$NumbersField = value;
		},
		get_operators: function() {
			return this.$1$OperatorsField;
		},
		set_operators: function(value) {
			this.$1$OperatorsField = value;
		},
		get_targetValue: function() {
			return this.$model.get_targetValue();
		},
		undo: function() {
			if (this.$stepsCount === 0) {
				this.$host.restorePreviousGame();
				return;
			}
			var number = this.$model.pop();
			if ($Numbers_Web_Number.op_Equality(number, null)) {
				return;
			}
			this.get_numbers().remove(Enumerable.from(this.get_numbers()).firstOrDefault(function(vm) {
				return $Numbers_Web_Number.op_Equality(vm.get_model(), number);
			}, ss.getDefaultValue($Numbers_Web_ViewModels_NumberViewModel)));
			var $t1 = ss.getEnumerator(this.get_operators());
			try {
				while ($t1.moveNext()) {
					var operatorViewModel = $t1.current();
					operatorViewModel.set_isSelected(false);
				}
			}
			finally {
				$t1.dispose();
			}
			var $t2 = this.get_numbers().getEnumerator();
			try {
				while ($t2.moveNext()) {
					var numberViewModel = $t2.current();
					numberViewModel.set_isSelected(false);
				}
			}
			finally {
				$t2.dispose();
			}
			ss.clear(this.$selectedNumbers);
			var operand1ViewModel = new $Numbers_Web_ViewModels_NumberViewModel(number.get_operand1(), false, 2);
			var operand2ViewModel = new $Numbers_Web_ViewModels_NumberViewModel(number.get_operand2(), false, 2);
			this.$insertNumber(operand1ViewModel);
			this.$insertNumber(operand2ViewModel);
		},
		hint: function() {
			var number = this.$model.hint();
			this.$hintUsed = true;
			if ($Numbers_Web_Number.op_Inequality(number, null)) {
				this.$push(number);
			}
			else {
				this.undo();
			}
		},
		newGame: function() {
			var levelChange = 1;
			if (this.$stepsCount > 0) {
				if (!this.$model.get_isSolved()) {
					levelChange = 0;
				}
				else if (!this.$hintUsed && this.$stepsCount < 20) {
					levelChange = 2;
				}
			}
			this.$host.newGame(levelChange);
		},
		$onItemsCollectionChanged: function(sender, e) {
			if (e.get_action() === 1) {
				ss.safeCast(e.get_item(), $Numbers_Web_ViewModels_NumberViewModel).remove_isSelectedChanged(ss.mkdel(this, this.$onNumberIsSelectedChanged));
			}
			else if (e.get_action() === 0) {
				ss.safeCast(e.get_item(), $Numbers_Web_ViewModels_NumberViewModel).add_isSelectedChanged(ss.mkdel(this, this.$onNumberIsSelectedChanged));
			}
			else {
				throw new ss.Exception('Unsupported collection action');
			}
		},
		$onNumberIsSelectedChanged: function(sender, e) {
			var numberViewModel = ss.safeCast(sender, $Numbers_Web_ViewModels_NumberViewModel);
			if (numberViewModel.get_isSelected()) {
				ss.add(this.$selectedNumbers, numberViewModel);
				if (this.$selectedNumbers.length > 2) {
					this.$selectedNumbers[0].set_isSelected(false);
				}
			}
			else {
				ss.remove(this.$selectedNumbers, numberViewModel);
			}
			this.$tryCalculate();
		},
		$onOperatorIsSelectedChanged: function(sender, e) {
			var senderOperatorViewModel = ss.safeCast(sender, $Numbers_Web_ViewModels_OperatorViewModel);
			if (senderOperatorViewModel.get_isSelected()) {
				var $t1 = ss.getEnumerator(this.get_operators());
				try {
					while ($t1.moveNext()) {
						var operatorViewModel = $t1.current();
						operatorViewModel.set_isSelected(ss.referenceEquals(operatorViewModel, senderOperatorViewModel));
					}
				}
				finally {
					$t1.dispose();
				}
			}
			this.$tryCalculate();
		},
		$tryCalculate: function() {
			var operatorViewModel = Enumerable.from(this.get_operators()).where(function(vm) {
				return vm.get_isSelected();
			}).firstOrDefault(null, ss.getDefaultValue($Numbers_Web_ViewModels_OperatorViewModel));
			var numberViewModels = ((Enumerable.from(this.get_numbers()).count() === 2) ? Enumerable.from(this.get_numbers()).toArray() : Enumerable.from(this.get_numbers()).where(function(vm1) {
				return vm1.get_isSelected();
			}).toArray());
			if (ss.isNullOrUndefined(operatorViewModel) || numberViewModels.length !== 2) {
				return;
			}
			var result = operatorViewModel.calculate(numberViewModels[0], numberViewModels[1]);
			if ($Numbers_Web_Number.op_Inequality(result, null)) {
				this.$push(result);
			}
		},
		$push: function(number) {
			this.$model.push(number);
			this.$stepsCount++;
			this.get_numbers().remove(Enumerable.from(this.get_numbers()).firstOrDefault(function(vm) {
				return $Numbers_Web_Number.op_Equality(vm.get_model(), number.get_operand1());
			}, ss.getDefaultValue($Numbers_Web_ViewModels_NumberViewModel)));
			this.get_numbers().remove(Enumerable.from(this.get_numbers()).firstOrDefault(function(vm1) {
				return $Numbers_Web_Number.op_Equality(vm1.get_model(), number.get_operand2());
			}, ss.getDefaultValue($Numbers_Web_ViewModels_NumberViewModel)));
			var $t1 = ss.getEnumerator(this.get_operators());
			try {
				while ($t1.moveNext()) {
					var operatorViewModel = $t1.current();
					operatorViewModel.set_isSelected(false);
				}
			}
			finally {
				$t1.dispose();
			}
			var $t2 = this.get_numbers().getEnumerator();
			try {
				while ($t2.moveNext()) {
					var numberViewModel = $t2.current();
					numberViewModel.set_isSelected(false);
				}
			}
			finally {
				$t2.dispose();
			}
			ss.clear(this.$selectedNumbers);
			var resultViewModel = new $Numbers_Web_ViewModels_NumberViewModel(number, this.get_numbers().get_count() === 0 && number.get_value() === this.get_targetValue(), 1);
			resultViewModel.set_isSelected(this.get_numbers().get_count() > 0);
			if (resultViewModel.get_isSelected()) {
				ss.add(this.$selectedNumbers, resultViewModel);
			}
			this.$insertNumber(resultViewModel);
			if (this.$model.get_isSolved()) {
				this.$raiseSolved();
			}
		},
		$insertNumber: function(numberViewModel) {
			var index = this.get_numbers().indexOf(Enumerable.from(this.get_numbers()).firstOrDefault(function(vm) {
				return vm.get_value() > numberViewModel.get_value();
			}, ss.getDefaultValue($Numbers_Web_ViewModels_NumberViewModel)));
			if (index === -1) {
				this.get_numbers().add(numberViewModel);
			}
			else {
				this.get_numbers().insert(index, numberViewModel);
			}
		},
		$raiseSolved: function() {
			if (!ss.staticEquals(this.$1$SolvedField, null)) {
				this.$1$SolvedField(this, ss.EventArgs.Empty);
			}
		}
	});
	ss.initClass($Numbers_Web_ViewModels_SelectableViewModel, $asm, {
		add_isSelectedChanged: function(value) {
			this.$1$IsSelectedChangedField = ss.delegateCombine(this.$1$IsSelectedChangedField, value);
		},
		remove_isSelectedChanged: function(value) {
			this.$1$IsSelectedChangedField = ss.delegateRemove(this.$1$IsSelectedChangedField, value);
		},
		get_isSelected: function() {
			return this.$isSelected;
		},
		set_isSelected: function(value) {
			if (this.$isSelected !== value) {
				this.$isSelected = value;
				this.$raiseIsSelectedChanged();
			}
		},
		$raiseIsSelectedChanged: function() {
			if (!ss.staticEquals(this.$1$IsSelectedChangedField, null)) {
				this.$1$IsSelectedChangedField(this, ss.EventArgs.Empty);
			}
		}
	});
	ss.initClass($Numbers_Web_ViewModels_NumberViewModel, $asm, {
		get_model: function() {
			return this.$2$ModelField;
		},
		set_model: function(value) {
			this.$2$ModelField = value;
		},
		get_value: function() {
			return this.get_model().get_value();
		},
		get_level: function() {
			return this.get_model().get_level();
		},
		get_isTarget: function() {
			return this.$2$IsTargetField;
		},
		set_isTarget: function(value) {
			this.$2$IsTargetField = value;
		},
		get_source: function() {
			return this.$2$SourceField;
		},
		set_source: function(value) {
			this.$2$SourceField = value;
		}
	}, $Numbers_Web_ViewModels_SelectableViewModel);
	ss.initClass($Numbers_Web_ViewModels_OperatorViewModel, $asm, {
		get_header: function() {
			return this.$2$HeaderField;
		},
		set_header: function(value) {
			this.$2$HeaderField = value;
		},
		calculate: function(a, b) {
			return this.$calculation(a.get_model(), b.get_model());
		}
	}, $Numbers_Web_ViewModels_SelectableViewModel);
	ss.initClass($Numbers_Web_Views_GameView, $asm, {
		run: function() {
			this.$numbersCollectionView.startAppearAnimation(600);
			this.$operatorsCollectionView.startAppearAnimation(600);
			this.$targetLabelAppearAnimation.start();
			this.$toolbarButtonsAppearAnimation.start();
			window.setTimeout(ss.mkdel(this, function() {
				this.$newGameButton.set_isEnabled(true);
			}), 1000);
		},
		$newGame: function() {
			if (this.$newGameRequested) {
				return;
			}
			this.$newGameRequested = true;
			if (this.$solved) {
				this.$solveDisappearAnimation.start();
				window.setTimeout(ss.mkdel(this.$viewModel, this.$viewModel.newGame), 2000);
			}
			else {
				this.$targetLabelDisappearAnimation.start();
				this.$toolbarButtonsDisappearAnimation.start();
				this.$numbersCollectionView.startDisappearAnimation(600);
				this.$operatorsCollectionView.startDisappearAnimation(600);
				window.setTimeout(ss.mkdel(this.$viewModel, this.$viewModel.newGame), 1000);
			}
		},
		$onSolved: function(sender, e) {
			this.$solved = true;
			this.$hintButton.set_isEnabled(false);
			this.$undoButton.set_isEnabled(false);
			this.$solveAppearAnimation.start();
		}
	}, $Numbers_Web_Controls_Control, [ss.IEnumerable]);
	ss.initClass($Numbers_Web_Views_NumbersCollectionView, $asm, {
		startAppearAnimation: function(totalAppearDurationMilliseconds) {
			var $t1 = this.$numbersButtons.getEnumerator();
			try {
				while ($t1.moveNext()) {
					var button = $t1.current();
					var start = ss.Int32.div(totalAppearDurationMilliseconds * button.get_left(), $Numbers_Web_Views_NumbersCollectionView.$numbersCollectionWidth);
					window.setTimeout(ss.mkdel(button, button.startScaleInAnimation), start);
				}
			}
			finally {
				$t1.dispose();
			}
		},
		startDisappearAnimation: function(totalDisappearDurationMilliseconds) {
			var $t1 = this.$numbersButtons.getEnumerator();
			try {
				while ($t1.moveNext()) {
					var button = $t1.current();
					var start = ss.Int32.div(totalDisappearDurationMilliseconds * button.get_left(), $Numbers_Web_Views_NumbersCollectionView.$numbersCollectionWidth);
					window.setTimeout(ss.mkdel(button, button.startScaleOutAnimation), start);
				}
			}
			finally {
				$t1.dispose();
			}
		},
		$updateLayout: function() {
			var left = ss.Int32.div($Numbers_Web_Views_NumbersCollectionView.$numbersCollectionWidth - Enumerable.from(this.$numbersButtons).count() * 88 + $Numbers_Web_Views_NumbersCollectionView.$numberMargin, 2);
			var $t1 = this.$numbersButtons.getEnumerator();
			try {
				while ($t1.moveNext()) {
					var button = $t1.current();
					button.set_left(left);
					button.get_shadow().set_left(left);
					left += 88;
				}
			}
			finally {
				$t1.dispose();
			}
		},
		$onNumbersButtonsCollectionChanged: function(sender, e) {
			if (e.get_action() === 0) {
				this.$addButton(ss.safeCast(e.get_item(), $Numbers_Web_Controls_Button));
			}
			else if (e.get_action() === 1) {
				this.$removeButton(ss.safeCast(e.get_item(), $Numbers_Web_Controls_Button));
			}
			else {
				throw new ss.Exception('Collection change action is not supported');
			}
			this.$updateLayout();
		},
		$addButton: function(button) {
			this.appendChild(button);
			this.appendChild(button.get_shadow());
		},
		$removeButton: function(button) {
			this.removeChild(button);
			this.removeChild(button.get_shadow());
		}
	}, $Numbers_Web_Controls_Control, [ss.IEnumerable]);
	ss.initClass($Numbers_Web_Views_OperatorsCollectionView, $asm, {
		startAppearAnimation: function(totalAppearDurationMilliseconds) {
			var $t1 = ss.getEnumerator(this.$operatorsButtons);
			try {
				while ($t1.moveNext()) {
					var button = $t1.current();
					var start = ss.Int32.div(totalAppearDurationMilliseconds * button.get_left(), $Numbers_Web_Views_OperatorsCollectionView.$operatorsCollectionWidth);
					window.setTimeout(ss.mkdel(button, button.startScaleInAnimation), start);
				}
			}
			finally {
				$t1.dispose();
			}
		},
		startDisappearAnimation: function(totalDisappearDurationMilliseconds) {
			var $t1 = ss.getEnumerator(this.$operatorsButtons);
			try {
				while ($t1.moveNext()) {
					var button = $t1.current();
					var start = ss.Int32.div(totalDisappearDurationMilliseconds * button.get_left(), $Numbers_Web_Views_OperatorsCollectionView.$operatorsCollectionWidth);
					window.setTimeout(ss.mkdel(button, button.startScaleOutAnimation), start);
				}
			}
			finally {
				$t1.dispose();
			}
		}
	}, $Numbers_Web_Controls_Control, [ss.IEnumerable]);
	$Numbers_Web_Configuration.$gmtTimeFormat = "ddd, dd MMM yyyy HH:mm:ss 'GMT'";
	$Numbers_Web_GameFactory.$minimumTarget = 40;
	$Numbers_Web_GameFactory.$maximumTarget = 401;
	$Numbers_Web_GameFactory.$targetMean = 200;
	$Numbers_Web_GameFactory.$targetMeanSd = 100;
	$Numbers_Web_GameFactory.$random = new ss.Random();
	$Numbers_Web_TokenDictionary.$tokenSeparator = 44;
	$Numbers_Web_TokenDictionary.$keyValueSeparator = 32;
	$Numbers_Web_Transitions_ScaleValueBounds.$scaleRegex = new RegExp('scale\\((.*)\\)');
	$Numbers_Web_Transitions_ScaleValueBounds.$matrixRegex = new RegExp('matrix\\( *(.*), *0, *0, *(.*),.*,.*\\)');
	$Numbers_Web_Transitions_TimingCurve.ease = new $Numbers_Web_Transitions_TimingCurve(0.25, 0.1, 0.25, 1, 'ease');
	$Numbers_Web_Transitions_TimingCurve.linear = new $Numbers_Web_Transitions_TimingCurve(0, 0, 1, 1, 'linear');
	$Numbers_Web_Transitions_TimingCurve.easeIn = new $Numbers_Web_Transitions_TimingCurve(0.42, 0, 1, 1, 'ease-in');
	$Numbers_Web_Transitions_TimingCurve.easeOut = new $Numbers_Web_Transitions_TimingCurve(0, 0, 0.58, 1, 'ease-out');
	$Numbers_Web_Transitions_TimingCurve.easeInOut = new $Numbers_Web_Transitions_TimingCurve(0.42, 0, 0.58, 1, 'ease-in-out');
	$Numbers_Web_Controls_Button.$checkAnimationDurationMilliseconds = 100;
	$Numbers_Web_Views_NumbersCollectionView.$numberWidth = 80;
	$Numbers_Web_Views_NumbersCollectionView.$numberMargin = 8;
	$Numbers_Web_Views_NumbersCollectionView.$numbersCollectionWidth = 600;
	$Numbers_Web_Views_OperatorsCollectionView.$operatorWidth = 80;
	$Numbers_Web_Views_OperatorsCollectionView.$operatorMargin = 8;
	$Numbers_Web_Views_OperatorsCollectionView.$operatorsCollectionWidth = 600;
	$Numbers_Web_Views_GameView.width = 600;
	$Numbers_Web_Views_GameView.height = 340;
	$Numbers_Web_Application.$levelMargins = 5;
	$Numbers_Web_Application.$defaultLevel = 50;
	$Numbers_Web_Application.$levelConfigurationKey = 'Level';
	$Numbers_Web_Application.main();
})();
