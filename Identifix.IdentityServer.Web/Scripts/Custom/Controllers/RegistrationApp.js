
angular.module("registrationApp", ['ngRoute'])
    .constant('UNITED_STATES_ID', 1)
    .controller("registrationCTRL", function ($scope, UNITED_STATES_ID, localServer, $location) {

        $scope.data = {
            newUser: {country: {id: UNITED_STATES_ID, name: "United States", code: "US"}, email:'' },
            editUser: {},
            chgPwd: {},
            verifyEmail: {},
            resetPassword: { tokenExpired: $location.search()['expired'] == "True", clientIdRedirect: $location.search()['signin'] }
        };

        localServer.errorCallback = function (error) {
            $scope.data.errorMessage = error;
        }

        localServer.addBackOfficeCommunicationCallback(function(operation, url) {
            if (operation === "DONE") {
                $scope.amBackEndProcessing = false;
            } else {
                $scope.amBackEndProcessing = true;
            }
        });

        $scope.validateNewUserEmail = function () {
            localServer.validateEmailAddress($scope.data.newUser.email, "", function (result) {
                //This function is a 'Success callback"
                if (result && result.payload == false) {
                    $scope.data.newUser.emailValidationError = localServer.lastWebAPIResult.errorMessage;
                } else {
                    $scope.data.newUser.emailValidationError = '';

                }
            });
        }
        
        $scope.validateEditUserEmail = function () {
            localServer.validateEmailAddress($scope.data.editUser.email, $scope.data.editUser.originalEmail, function (result) {
                //This function is a 'Success callback"
                if (result && result.payload == false) {
                    $scope.data.editUser.emailValidationError = localServer.lastWebAPIResult.errorMessage;
                } else {
                    $scope.data.editUser.emailValidationError = '';
                }
            });
        }
        
        localServer.retrieveLoggedInUserCallback = function (currentUser) {
            currentUser.confirmEmailAddress = currentUser.email;
            $scope.data.editUser = currentUser;
        };

        $scope.verifyPasswordIsValid = function () {
            var password;
            switch($location.path())
            {
                case '/changePassword':
                    password = $scope.data.chgPwd.newPassword
                    break;
                case '/resetPassword':
                    password = $scope.data.resetPassword.newPassword;
                    break;
                default:
                    password = $scope.data.newUser.password;
                    break;
            }

            localServer.validatePassword(password, function (result) {
                if (!result.payload) {
                    $scope.data.chgPwd.invalidNewPasswordError = result.errorMessage;
                    $scope.data.newUser.invalidPasswordError = result.errorMessage;
                    $scope.data.resetPassword.invalidNewPasswordError = result.errorMessage;
                }
                else {
                    $scope.data.chgPwd.invalidNewPasswordError = "";
                    $scope.data.newUser.invalidPasswordError = "";
                    $scope.data.resetPassword.invalidNewPasswordError = "";
                }
            })
        }


        $scope.referenceData = { countries: localServer.countries };

        $scope.getStates = function () {
            var result = localServer.availableStates($scope.data.newUser.country);

            //Used for the states dropdown list ng-require
            $scope.selectedCountryHasStates = result.length > 0;

            return result;
        }

        $scope.updateUser = function (isFormValid) {
            var isValid = isFormValid && ($scope.data.editUser.emailValidationError == "" || $scope.data.editUser.emailValidationError == null || $scope.data.editUser.emailValidationError == undefined);
            $scope.invalidEditUserSubmittal = isValid === false;

            if (!$scope.invalidEditUserSubmittal) {
                localServer.updateUserProfile($scope.data.editUser, function () {
                    $scope.data.saveDataMessage = "Your profile has been upated";
                });
            }
        }

        $scope.registerNewUser = function (isFormValid) {
            var isValid = isFormValid && ($scope.data.newUser.emailValidationError == "" || $scope.data.newUser.emailValidationError == null || $scope.data.newUser.emailValidationError == undefined);
            $scope.invalidNewUserSubmittal = isValid === false;

            if (!$scope.invalidNewUserSubmittal) {
                localServer.registerNewUser($scope.data.newUser);
            }

        }

        $scope.changePassword = function (isFormValid) {
            $scope.data.errorMessage = "";

            if (isFormValid) {
                localServer.changePassword($scope.data.chgPwd,
                    function () {
                        $scope.data.saveDataMessage = "Your password has been changed";
                    },
                    function (errorMessage) {
                        $scope.data.chgPwd = {};
                        $scope.data.errorMessage = errorMessage;
                    });
            }
        }

        $scope.resetPassword = function () {
            localServer.requestPasswordResetLink($scope.data.resetPassword, function (result) {
                $scope.data.errorMessage = "";
                $scope.data.resetPassword.emailFileName = result.payload;
                $scope.data.resetPassword.resetPasswordLinkGenerated = true;
            });
        }

        $scope.resetPasswordConfirmation = function () {
            $scope.data.resetPassword.token = $location.search()["token"];
            $scope.data.resetPassword.signInToken = $location.search()["signin"];
            localServer.resetPassword($scope.data.resetPassword);
        }

        $scope.navigateTo = function (subResource) {
            localServer.navigateTo(subResource);
        }

    });