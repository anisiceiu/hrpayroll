/*
 * HR & Payroll Management System - Dashboard JavaScript
 */

$(function() {
    'use strict';

    // Initialize dashboard charts and components
    initAttendanceChart();
    initDepartmentChart();
    initDatePickers();
    initDataTables();
    initNotifications();
});

// Attendance Line Chart
function initAttendanceChart() {
    var attendanceCtx = document.getElementById('attendanceChart').getContext('2d');
    
    if (attendanceCtx) {
        new Chart(attendanceCtx, {
            type: 'line',
            data: {
                labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
                datasets: [{
                    label: 'Present',
                    data: [145, 150, 148, 155, 152, 156],
                    borderColor: '#28a745',
                    backgroundColor: 'rgba(40, 167, 69, 0.1)',
                    borderWidth: 2,
                    tension: 0.3,
                    fill: true
                }, {
                    label: 'Absent',
                    data: [5, 3, 6, 2, 4, 3],
                    borderColor: '#dc3545',
                    backgroundColor: 'rgba(220, 53, 69, 0.1)',
                    borderWidth: 2,
                    tension: 0.3,
                    fill: true
                }, {
                    label: 'Leave',
                    data: [8, 6, 7, 5, 8, 8],
                    borderColor: '#ffc107',
                    backgroundColor: 'rgba(255, 193, 7, 0.1)',
                    borderWidth: 2,
                    tension: 0.3,
                    fill: true
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'bottom'
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    }
}

// Department Distribution Doughnut Chart
function initDepartmentChart() {
    var departmentCtx = document.getElementById('departmentChart').getContext('2d');
    
    if (departmentCtx) {
        new Chart(departmentCtx, {
            type: 'doughnut',
            data: {
                labels: ['Engineering', 'Marketing', 'HR', 'Finance', 'Operations', 'Sales'],
                datasets: [{
                    data: [45, 20, 8, 12, 25, 30],
                    backgroundColor: [
                        '#4a6fa5',
                        '#28a745',
                        '#dc3545',
                        '#ffc107',
                        '#17a2b8',
                        '#6c757d'
                    ],
                    borderWidth: 2,
                    borderColor: '#fff'
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'bottom'
                    }
                }
            }
        });
    }
}

// Initialize date pickers
function initDatePickers() {
    $('.date-picker').datepicker({
        format: 'yyyy-mm-dd',
        autoclose: true,
        todayHighlight: true
    });
}

// Initialize data tables
function initDataTables() {
    if ($.fn.DataTable) {
        $('.datatable').DataTable({
            'paging': true,
            'lengthChange': true,
            'searching': true,
            'ordering': true,
            'info': true,
            'autoWidth': false,
            'responsive': true
        });
    }
}

// Initialize notifications
function initNotifications() {
    // Mark notifications as read on click
    $('.notification-item').on('click', function() {
        $(this).removeClass('unread');
    });
    
    // Update notification badge
    updateNotificationBadge();
}

// Update notification badge count
function updateNotificationBadge() {
    var unreadCount = $('.notification-item.unread').length;
    $('.notification-badge .badge').text(unreadCount);
}

// Common utility functions
var HRApp = {
    // Show loading spinner
    showLoading: function(element) {
        $(element).html('<div class="text-center"><i class="fas fa-spinner fa-spin fa-2x"></i></div>');
    },
    
    // Hide loading spinner
    hideLoading: function(element) {
        $(element).html('');
    },
    
    // Show toast notification
    showToast: function(message, type = 'success') {
        var toastClass = type === 'success' ? 'bg-success' : 'bg-danger';
        $('body').append(`
            <div class="toast ${toastClass}" style="position: fixed; top: 20px; right: 20px; z-index: 9999;">
                <div class="toast-header ${toastClass} text-white">
                    <strong class="mr-auto">Notification</strong>
                    <button type="button" class="ml-2 mb-1 close" data-dismiss="toast">&times;</button>
                </div>
                <div class="toast-body text-white">${message}</div>
            </div>
        `);
        $('.toast').toast('show');
    },
    
    // Confirm dialog
    confirmDelete: function(message, callback) {
        if (confirm(message)) {
            callback();
        }
    },
    
    // Format currency
    formatCurrency: function(amount, currency = '$') {
        return currency + parseFloat(amount).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
    },
    
    // Format date
    formatDate: function(date, format = 'YYYY-MM-DD') {
        var d = new Date(date);
        var year = d.getFullYear();
        var month = ('0' + (d.getMonth() + 1)).slice(-2);
        var day = ('0' + d.getDate()).slice(-2);
        return format.replace('YYYY', year).replace('MM', month).replace('DD', day);
    },
    
    // Calculate age
    calculateAge: function(birthdate) {
        var today = new Date();
        var birthDate = new Date(birthdate);
        var age = today.getFullYear() - birthDate.getFullYear();
        var monthDiff = today.getMonth() - birthDate.getMonth();
        if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
            age--;
        }
        return age;
    }
};

// Export functions
var ExportUtils = {
    // Export table to CSV
    exportToCSV: function(tableId, filename) {
        var table = document.getElementById(tableId);
        var rows = table.querySelectorAll('tr');
        var csv = [];
        
        for (var i = 0; i < rows.length; i++) {
            var row = [], cols = rows[i].querySelectorAll('td, th');
            for (var j = 0; j < cols.length; j++) {
                row.push('"' + cols[j].innerText + '"');
            }
            csv.push(row.join(','));
        }
        
        var csvFile = new Blob([csv.join('\n')], {type: 'text/csv'});
        var downloadLink = document.createElement('a');
        downloadLink.download = filename + '.csv';
        downloadLink.href = window.URL.createObjectURL(csvFile);
        downloadLink.style.display = 'none';
        document.body.appendChild(downloadLink);
        downloadLink.click();
    },
    
    // Export table to Excel
    exportToExcel: function(tableId, filename) {
        ExportUtils.exportToCSV(tableId, filename);
        // Could be enhanced with a library like SheetJS
    },
    
    // Print table
    printTable: function(tableId) {
        var table = document.getElementById(tableId);
        var printWindow = window.open('', '', 'height=600,width=800');
        printWindow.document.write('<html><head><title>Print</title>');
        printWindow.document.write('<link rel="stylesheet" href="plugins/bootstrap/css/bootstrap.min.css">');
        printWindow.document.write('</head><body>');
        printWindow.document.write(table.outerHTML);
        printWindow.document.write('</body></html>');
        printWindow.document.close();
        printWindow.print();
    }
};

// Validation utilities
var ValidationUtils = {
    // Validate email
    isValidEmail: function(email) {
        var re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(email);
    },
    
    // Validate phone
    isValidPhone: function(phone) {
        var re = /^[0-9]{10,15}$/;
        return re.test(phone.replace(/[^0-9]/g, ''));
    },
    
    // Validate required field
    isRequired: function(value) {
        return value !== null && value !== undefined && value.trim() !== '';
    },
    
    // Show field error
    showFieldError: function(field, message) {
        field.addClass('is-invalid');
        field.after('<div class="invalid-feedback">' + message + '</div>');
    },
    
    // Clear field error
    clearFieldError: function(field) {
        field.removeClass('is-invalid');
        field.next('.invalid-feedback').remove();
    }
};

// AJAX helper
var AjaxUtils = {
    // GET request
    get: function(url, data, successCallback, errorCallback) {
        $.ajax({
            url: url,
            type: 'GET',
            data: data,
            success: successCallback,
            error: errorCallback || function(xhr, status, error) {
                console.error('AJAX Error:', error);
                HRApp.showToast('An error occurred. Please try again.', 'error');
            }
        });
    },
    
    // POST request
    post: function(url, data, successCallback, errorCallback) {
        $.ajax({
            url: url,
            type: 'POST',
            data: data,
            success: successCallback,
            error: errorCallback || function(xhr, status, error) {
                console.error('AJAX Error:', error);
                HRApp.showToast('An error occurred. Please try again.', 'error');
            }
        });
    },
    
    // POST with JSON data
    postJSON: function(url, data, successCallback, errorCallback) {
        $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: successCallback,
            error: errorCallback || function(xhr, status, error) {
                console.error('AJAX Error:', error);
                HRApp.showToast('An error occurred. Please try again.', 'error');
            }
        });
    }
};

// Form utilities
var FormUtils = {
    // Get form data as object
    getData: function(formId) {
        var formData = {};
        $('#' + formId).serializeArray().forEach(function(field) {
            formData[field.name] = field.value;
        });
        return formData;
    },
    
    // Reset form
    reset: function(formId) {
        $('#' + formId)[0].reset();
        $('.is-invalid').removeClass('is-invalid');
        $('.invalid-feedback').remove();
    },
    
    // Populate form
    populate: function(formId, data) {
        $.each(data, function(key, value) {
            var field = $('#' + formId + ' [name="' + key + '"]');
            if (field.length) {
                field.val(value);
            }
        });
    }
};
