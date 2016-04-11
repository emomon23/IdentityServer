angular.module("registrationApp")
.config(function ($routeProvider) {
    $routeProvider.when("/emailVerification",
        { templateUrl: "/User/EmailVerification.html" });

    $routeProvider.when('/changePassword',
        { templateUrl: "/User/ChangePassword.html" });

    $routeProvider.when('/update',
       { templateUrl: "/User/UpdteProfile.html" });

    $routeProvider.when('/saved',
       { templateUrl: "/User/DataSaved.html" });

    $routeProvider.when('/confirmEmail',
        { templateUrl: '/User/CheckYourEmailForConfirmation.html' });

    $routeProvider.when('/generateResetPasswordLink',
        { templateUrl: '/User/GenerateResetPasswordLink.html' });

    $routeProvider.when('/resetPassword',
    { templateUrl: '/User/ResetPassword.html' });

    $routeProvider.otherwise({
        templateUrl: "/User/newUserRegistration.html"
    });
});