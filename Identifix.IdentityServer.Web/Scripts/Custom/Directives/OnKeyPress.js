var onKeyPress = function () {
    return {
        scope: true,
        link: function (scope, element, attributes, ngModel) {

            var scopeMethodToCall = attributes.onKeyPress;

            $(element).bind('keypress', elementKeyPress);
            
            function elementKeyPress(event) {
                var functionPtr = scope[scopeMethodToCall];
                functionPtr(event);
            }

            scope.$on("$destroy", function() {
                $(element).unbind('keypress', elementKeyPress);
            });
        }
      };
};

angular.module("registrationApp")
    .directive("onKeyPress", onKeyPress);