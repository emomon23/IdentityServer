var compareTo = function () {
    return {
        require: "ngModel",
        scope: {
            otherModelValue: "=compareTo"
        },
        link: function (scope, element, attributes, ngModel) {

            ngModel.$validators.compareTo = function (modelValue) {
                return !modelValue || modelValue == '' || modelValue == scope.otherModelValue;
            };

            element.bind("blur", blurFnc);

            var blurFnc = function () { ngModel.$validate(); }

            scope.$watch("otherModelValue", blurFnc);

            //avoid a memory leak
            scope.$on("$destroy", function handler() {
                element.removeEventListener("blur", blurFnc);
            });
        }
    };
};

angular.module("registrationApp")
    .directive("compareTo", compareTo);