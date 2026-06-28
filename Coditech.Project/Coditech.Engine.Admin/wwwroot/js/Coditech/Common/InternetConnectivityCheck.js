/**
 * Internet Connectivity Check Utility
 * Checks if internet is available and displays appropriate messages
 */
var InternetConnectivityCheck = {
    isConnected: navigator.onLine,
    checkInterval: null,
    // Use a reliable external lightweight endpoint that returns 204 for connectivity checks
    externalEndpoint: 'https://www.gstatic.com/generate_204',
    apiEndpoint: '/api/connectivity/ping',
    requestTimeoutMs: 5000,

    /**
     * Initialize internet connectivity check
     */
    Initialize: function () {
        this.isConnected = navigator.onLine;

        // Setup global click prevention for disabled anchors
        this.setupDisabledAnchorHandler();

        // If browser knows it's offline, mark offline immediately
        if (!navigator.onLine) {
            this.handleOffline();
            // still start periodic checks to detect restoration
            this.startPeriodicCheck();
            return;
        }

        // Listen for online/offline events
        window.addEventListener('online', () => this.handleOnline());
        window.addEventListener('offline', () => this.handleOffline());

        // Perform an immediate connectivity check using HTTP requests
        this.performConnectivityCheck();

        // Also perform periodic checks with actual HTTP request
        this.startPeriodicCheck();
    },

    /**
     * Handle when internet comes online
     */
    handleOnline: function () {
        this.isConnected = true;
        // Clear any offline notifications and hide offline badge
        this.clearNotification();
        this.setInteractiveDisabled(false);
        console.log("Internet connection restored");
    },

    /**
     * Handle when internet goes offline
     */
    handleOffline: function () {
        this.isConnected = false;
        this.showNoInternetNotification();
        this.setInteractiveDisabled(true);
        console.log("Internet connection lost");
    },

    /**
     * Start periodic internet connectivity check
     */
    startPeriodicCheck: function () {
        var self = this;
        // Check every 30 seconds
        this.checkInterval = setInterval(function () {
            self.performConnectivityCheck();
        }, 30000);
    },

    /**
     * Stop periodic check
     */
    stopPeriodicCheck: function () {
        if (this.checkInterval) {
            clearInterval(this.checkInterval);
            this.checkInterval = null;
        }
    },

    /**
     * Perform actual connectivity check with HTTP request and timeouts
     */
    performConnectivityCheck: async function () {
        var self = this;

        // If browser reports offline, trust that immediately
        if (!navigator.onLine) {
            this.handleOffline();
            return;
        }

        // Helper to fetch with timeout
        function fetchWithTimeout(url, options, timeoutMs) {
            return new Promise(function (resolve, reject) {
                var controller = new AbortController();
                var timer = setTimeout(function () {
                    controller.abort();
                }, timeoutMs);

                fetch(url, Object.assign({}, options || {}, { signal: controller.signal, cache: 'no-store' }))
                    .then(function (res) {
                        clearTimeout(timer);
                        resolve(res);
                    })
                    .catch(function (err) {
                        clearTimeout(timer);
                        reject(err);
                    });
            });
        }

        try {
            // Try reliable external endpoint first
            try {
                var resp = await fetchWithTimeout(this.externalEndpoint, { method: 'GET', mode: 'no-cors' }, this.requestTimeoutMs);
                // When using no-cors, browsers often return an opaque response; treat as success if no exception
                if (resp) {
                    // mark online
                    if (!self.isConnected) {
                        self.isConnected = true;
                    }
                    // clear offline notification/badge
                    self.clearNotification();
                    self.setInteractiveDisabled(false);
                    console.log('Connectivity check: external endpoint responded');
                    return;
                }
            } catch (ex) {
                console.warn('External endpoint check failed:', ex);
                // fall through to local API check
            }

            // Try local API endpoint as fallback
            try {
                var apiResp = await fetchWithTimeout(this.apiEndpoint, { method: 'GET', mode: 'same-origin' }, this.requestTimeoutMs);
                if (apiResp && apiResp.ok) {
                    if (!self.isConnected) {
                        self.isConnected = true;
                    }
                    self.clearNotification();
                    self.setInteractiveDisabled(false);
                    console.log('Connectivity check: local API responded');
                    return;
                }
            } catch (ex) {
                console.warn('Local API check failed:', ex);
            }

            // If both checks failed, mark offline
            if (self.isConnected) {
                self.isConnected = false;
            }
            self.showNoInternetNotification();
            self.setInteractiveDisabled(true);
            console.log('Connectivity check: offline');
        } catch (err) {
            console.error('Connectivity check unexpected error:', err);
            self.isConnected = false;
            self.showNoInternetNotification();
            self.setInteractiveDisabled(true);
        }
    },

    /**
     * Show no internet notification
     */
    showNoInternetNotification: function () {
        // Check if notification system exists
        if (typeof CoditechNotification !== 'undefined') {
            CoditechNotification.DisplayNotificationMessage(
                "No Internet Connection. Please check your connection and try again.",
                "warning"
            );
        } else {
            // Fallback to alert
            console.warn("No Internet Connection available");
            this.showFallbackNotification();
        }

        // show offline badge only
        this.updateStatusBadge(false, 'Offline');
    },

    /**
     * Clear no internet notification
     */
    clearNotification: function () {
        // Remove notification if displayed
        var notification = document.getElementById('internet-connectivity-alert');
        if (notification) {
            notification.remove();
        }

        // hide offline badge
        try {
            var wrapper = document.getElementById('internet-connectivity-status');
            if (wrapper) wrapper.style.display = 'none';
        } catch (e) { }
    },

    /**
     * Fallback notification display
     */
    showFallbackNotification: function () {
        // Check if notification already exists
        if (document.getElementById('internet-connectivity-alert')) {
            return;
        }

        var alertDiv = document.createElement('div');
        alertDiv.id = 'internet-connectivity-alert';
        alertDiv.className = 'alert alert-danger alert-dismissible fade show';
        alertDiv.setAttribute('role', 'alert');
        alertDiv.innerHTML = `
            <strong>? No Internet Connection!</strong>
            <p>It appears you have lost your internet connection. Please check your connection and try again.</p>
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        `;
        alertDiv.style.cssText = 'position: fixed; top: 0; left: 0; right: 0; z-index: 9999; margin: 0; border-radius: 0;';

        document.body.insertBefore(alertDiv, document.body.firstChild);

        // Auto-remove after 10 seconds if connection is restored
        setTimeout(() => {
            if (this.isConnected && document.getElementById('internet-connectivity-alert')) {
                document.getElementById('internet-connectivity-alert').remove();
            }
        }, 10000);
    },

    /**
     * Disable or enable interactive elements on the page when offline/online
     * Elements can opt-out by adding `data-ignore-offline="true"`
     */
    setInteractiveDisabled: function (isDisabled) {
        try {
            // Select all button-like controls across the page
            var controls = Array.from(document.querySelectorAll('button, input[type="button"], input[type="submit"], input[type="reset"], textarea, select'));

            controls.forEach(function (el) {
                // Allow explicit opt-out
                if (el.dataset && el.dataset.ignoreOffline === 'true') return;

                // Do NOT disable controls that belong to topbar/navigation to avoid hiding essential UI
                if (el.closest && (el.closest('.topbar') || el.closest('.navbar-custom') || el.closest('.brand') || el.closest('.logo') || el.closest('header') || el.closest('.left-sidenav'))) {
                    return;
                }

                try {
                    el.disabled = isDisabled;
                } catch (e) { }

                if (isDisabled) {
                    el.classList.add('disabled', 'offline-disabled');
                    el.setAttribute('aria-disabled', 'true');
                } else {
                    el.classList.remove('disabled', 'offline-disabled');
                    el.removeAttribute('aria-disabled');
                }
            });

            // Also disable anchor elements (hyperlinks)
            var anchors = Array.from(document.querySelectorAll('a[href]'));
            anchors.forEach(function (a) {
                // allow explicit opt-out
                if (a.dataset && a.dataset.ignoreOffline === 'true') return;

                // Skip purely essential header anchors (logo/profile) but DO disable navigation/menu anchors
                var isInTopbar = a.closest && (a.closest('.topbar') || a.closest('.navbar-custom') || a.closest('.brand') || a.closest('.logo') || a.closest('header'));
                var isNavigationMenu = a.closest && (a.closest('.navigation-menu') || a.closest('#navigation') || a.closest('.left-sidenav'));
                if (isInTopbar && !isNavigationMenu) {
                    // keep header anchors like logo/profile usable
                    return;
                }

                // If anchor has href like '#', javascript:void(0) or is a modal toggle, skip disabling
                var href = (a.getAttribute('href') || '').trim();
                var skipHrefPatterns = ['#', 'javascript:void(0)', 'javascript:;'];
                if (skipHrefPatterns.indexOf(href) !== -1) return;

                if (isDisabled) {
                    // backup href and remove it
                    if (!a.dataset._hrefBackup) a.dataset._hrefBackup = href;
                    try { a.removeAttribute('href'); } catch (e) { }
                    a.classList.add('disabled', 'offline-disabled');
                    a.setAttribute('aria-disabled', 'true');
                } else {
                    // restore href
                    a.classList.remove('disabled', 'offline-disabled');
                    a.removeAttribute('aria-disabled');
                    if (a.dataset._hrefBackup !== undefined) {
                        try { a.setAttribute('href', a.dataset._hrefBackup); } catch (e) { }
                        delete a.dataset._hrefBackup;
                    }
                }
            });
        } catch (e) {
            // ignore errors
        }
    },

    /**
     * Prevent clicks on anchors that are marked aria-disabled
     */
    setupDisabledAnchorHandler: function () {
        document.addEventListener('click', function (e) {
            var target = e.target;
            // Walk up to find an anchor element
            while (target && target !== document) {
                if (target.tagName && target.tagName.toLowerCase() === 'a') break;
                target = target.parentNode;
            }
            if (!target || target === document) return;
            try {
                if (target.getAttribute('aria-disabled') === 'true') {
                    e.preventDefault();
                    e.stopPropagation();
                    // Optionally show notification when user attempts action while offline
                    if (typeof CoditechNotification !== 'undefined') {
                        CoditechNotification.DisplayNotificationMessage('Action unavailable while offline.', 'warning');
                    }
                    return false;
                }
            } catch (ex) {
                // ignore
            }
        }, true);
    },

    /**
     * Check if currently connected to internet
     */
    isOnline: function () {
        return this.isConnected;
    },

    /**
     * Update the status badge element on the page (only show when offline)
     */
    updateStatusBadge: function (isConnected, text) {
        try {
            var wrapper = document.getElementById('internet-connectivity-status');
            var badge = document.getElementById('internet-connectivity-badge');
            if (!wrapper || !badge) return;

            if (isConnected) {
                // hide badge when online
                wrapper.style.display = 'none';
                return;
            }

            // show badge only when offline
            badge.textContent = text || 'Offline';
            badge.classList.remove('bg-success');
            badge.classList.remove('bg-danger');
            badge.classList.add('bg-warning');
            wrapper.style.display = '';
        } catch (e) {
            // ignore
        }
    }
}

// Initialize when document is ready
document.addEventListener('DOMContentLoaded', function () {
    InternetConnectivityCheck.Initialize();
});
