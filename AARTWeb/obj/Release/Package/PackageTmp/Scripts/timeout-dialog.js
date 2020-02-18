/*
 * timeout-dialog.js v2.0.0, 10-19-2014
 * 
 * @original-author: Rodrigo Neri (@rigoneri)
 * 
 * (The MIT License)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE. 
 */


/* String formatting, you might want to remove this if you already use it.
 * Example:
 * 
 * var location = 'World';
 * alert('Hello {0}'.format(location));
 */
String.prototype.format = function() {
	var s = this,
			i = arguments.length;

	while (i--) {
		s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
	}
	return s;
};
var sessionout;
!function($) {
	$.timeoutDialog = {
		settings: {
            //timeout: 30,
            //countdown: 20,
			title: 'Your session is about to expire!',
			message: 'You will be logged out in {0} seconds.',
			question: 'Do you want to stay signed in?',
			keep_alive_button_text: 'Yes, Keep me signed in',
			sign_out_button_text: 'No, Sign me out',
			keep_alive_url: '',
			keep_alive_function: function() {
			},
			logout_url: null,
			logout_redirect_url: '/',
			logout_function: function() {
			},
			restart_on_yes: true,
			dialog_width: 350
		},
		alertSetTimeoutHandle: 0,
		setupDialogTimer: function(options) {
			if (options !== undefined) {
				$.extend(this.settings, options);
			}
			
			var self = this;
			if (self.alertSetTimeoutHandle !== 0) {
				clearTimeout(self.alertSetTimeoutHandle);
			}
           

            self.alertSetTimeoutHandle = window.setInterval(function () {
                //window.setTimeout(getsession(function () {
                //    console.log('huzzah, I\'m done!');
                //}), 1000)
                var getsessionvalue = getsession(function () {
                    console.log('huzzah, I\'m done!');
                });
                alert(getsessionvalue);
                if ((getsessionvalue - 20) <= 20) {
                    self.setupDialog();
                }
            }, ( this.settings.timeout - this.settings.countdown) * 1000);
		},
		setupDialog: function() {
			var self = this;
			self.destroyDialog();

            $('<div id="timeout-dialog">' +
                '<p id="timeout-message">' + this.settings.message.format('<span id="timeout-countdown">' + this.settings.countdown + '</span>') + '</p>' +
                '<p id="timeout-question">' + this.settings.question + '</p>' +
                '</div>')
                .appendTo('body')
                .kendoDialog({
                    modal: true,
                    width: this.settings.dialog_width,
                    minHeight: 'auto',
                    zIndex: 10000,
                    closable: false,
                    closeOnEscape: false,
                    draggable: false,
                    resizable: false,
                    dialogClass: 'timeout-dialog',
                    title: this.settings.title,
                    actions: [
                        {
                            text: 'Yes, Keep me signed in',id:"keepmesignin", action: function () {
                                self.keepAlive();
                            }},
                        {
                            text: 'No, Sign me out', primary: true, action: function () {
                                self.signOut();
                            }
                        }
                        //'keep-alive-button': {
                        //	text: this.settings.keep_alive_button_text,
                        //	id: "timeout-keep-signin-btn",
                        //	click: function() {
                        //		self.keepAlive();
                        //	}
                        //},
                        //'sign-out-button': {
                        //	text: this.settings.sign_out_button_text,
                        //	id: "timeout-sign-out-button",
                        //	click: function() {
                        //		self. (true);
                        //	}
                        //}
                    ]
                });
					//.dialog({
					//	modal: true,
					//	width: this.settings.dialog_width,
					//	minHeight: 'auto',
					//	zIndex: 10000,
					//	closeOnEscape: false,
					//	draggable: false,
					//	resizable: false,
					//	dialogClass: 'timeout-dialog',
					//	title: this.settings.title,
					//	buttons: {
					//		'keep-alive-button': {
					//			text: this.settings.keep_alive_button_text,
					//			id: "timeout-keep-signin-btn",
					//			click: function() {
					//				self.keepAlive();
					//			}
					//		},
					//		'sign-out-button': {
					//			text: this.settings.sign_out_button_text,
					//			id: "timeout-sign-out-button",
					//			click: function() {
					//				self.signOut(true);
					//			}
					//		}
					//	}
					//});

			self.startCountdown();
		},
		destroyDialog: function() {
			if ($("#timeout-dialog").length) {
                $("#timeout-dialog").kendoDialog("close");
				$('#timeout-dialog').remove();
			}
		},
		startCountdown: function() {
			var self = this,
					counter = this.settings.countdown;

			this.countdown = window.setInterval(function() {
				counter -= 1;
				$("#timeout-countdown").html(counter);

				if (counter <= 0) {
                    window.clearInterval(self.countdown);
                    $("#timeout-dialog").html("Your session is expired. Please login again.");
                    var divs = document.querySelectorAll('.k-dialog .k-button');
                  
                    for (var i = 0; i < divs.length; i++) {
                      
                        if (divs[i].innerText.indexOf("Yes, Keep me signed in") != -1) {                           
                            divs[i].innerText=divs[i].innerText.replace("Yes, Keep me signed in", "Sign In Again");
                        }
                        if (divs[i].innerText.indexOf("No, Sign me out") != -1) {
                            divs[i].style.visibility = 'hidden';
                        }
                    }               
                    $.ajax({
                        url: "../Account/sessionexpire",
                        type: 'get',
                        success: function (rs) {
                            //username = rs;
                        },
                        error: function (err) {
                            swal(err);
                        }
                    });
                    sessionout = 'true';
				}

			}, 1000);
		},
        keepAlive: function () {
            //alert("keepAlive");
			var self = this;
			this.destroyDialog();
			window.clearInterval(this.countdown);

			//this.settings.keep_alive_function();
           // alert(this.settings.keep_alive_url);
            if (sessionout == 'true') {
                self.signOut(false);

            }
            else {
                self.setupDialogTimer();

            }
            //if (this.settings.keep_alive_url !== '') {
            //   alert(this.settings.keep_alive_url);

            //    $.get(this.settings.keep_alive_url, function (data) {
            //      //  alert(data);
            //        if (data === "OK") {
            //            if (self.settings.restart_on_yes) {
            //                self.setupDialogTimer();
            //            }
            //        }
            //        else {
            //            self.signOut(false);
            //        }
            //    });
            //}
            //else {
            //   // alert("aadsa");
            //    self.signOut(false);

            //}
		},
		signOut: function(is_forced) {
			var self = this;
			this.destroyDialog();

			this.settings.logout_function(is_forced);

			if (this.settings.logout_url !== null) {
				$.post(this.settings.logout_url, function(data) {
					self.redirectLogout(is_forced);
				});
			}
			else {
				self.redirectLogout(is_forced);
			}
		},
		redirectLogout: function(is_forced) {
			var target = this.settings.logout_redirect_url + '?next=' + encodeURIComponent(window.location.pathname + window.location.search);
			if (!is_forced)
				target += '&timeout=t';
			window.location = target;
		},
    };
  

}(window.jQuery);   