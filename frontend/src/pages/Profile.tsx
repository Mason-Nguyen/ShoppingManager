import React, { useState } from 'react';
import Layout from '../components/Layout';
import { useAuth } from '../context/AuthContext';
import ChangePasswordModal from '../components/ChangePasswordModal';
import { UserRole } from '../types';

const Profile: React.FC = () => {
  const { user } = useAuth();
  const [showChangePasswordModal, setShowChangePasswordModal] = useState<boolean>(false);
  const [successMessage, setSuccessMessage] = useState<string>('');

  const getRoleName = (role: number | undefined): string => {
    if (role === undefined) return 'Unknown';
    
    const roleNames: Record<number, string> = {
      [UserRole.User]: 'User',
      [UserRole.Admin]: 'Admin',
      [UserRole.Requester]: 'Requester',
      [UserRole.Approver]: 'Approver',
      [UserRole.Receiver]: 'Receiver',
      [UserRole.Purchase]: 'Purchase',
    };
    
    return roleNames[role] || 'Unknown';
  };

  const getRoleDescription = (role: number | undefined): string => {
    if (role === undefined) return 'Standard user access';
    
    const descriptions: Record<number, string> = {
      [UserRole.Admin]: 'Full system access with user management capabilities',
      [UserRole.Requester]: 'Can create and submit shopping requests',
      [UserRole.Approver]: 'Can review and approve shopping requests',
      [UserRole.Receiver]: 'Can receive and confirm delivered items',
      [UserRole.Purchase]: 'Can process approved requests and make purchases',
      [UserRole.User]: 'Basic user with limited access',
    };
    
    return descriptions[role] || 'Standard user access';
  };

  const handleChangePasswordSuccess = (): void => {
    setSuccessMessage('Password changed successfully!');
    setTimeout(() => setSuccessMessage(''), 5000);
  };

  if (!user) {
    return (
      <Layout>
        <div className="flex justify-center items-center h-64">
          <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-blue-500"></div>
        </div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="max-w-4xl mx-auto space-y-6">
        {successMessage && (
          <div className="bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded">
            {successMessage}
          </div>
        )}

        <div className="bg-white shadow overflow-hidden sm:rounded-lg">
          <div className="px-4 py-5 sm:p-6">
            <h1 className="text-2xl font-bold text-gray-900 mb-6">User Profile</h1>
            
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              {/* Personal Information */}
              <div>
                <h3 className="text-lg leading-6 font-medium text-gray-900 mb-4">
                  Personal Information
                </h3>
                <dl className="space-y-3">
                  <div>
                    <dt className="text-sm font-medium text-gray-500">Full Name</dt>
                    <dd className="text-sm text-gray-900">{user.firstName} {user.lastName}</dd>
                  </div>
                  <div>
                    <dt className="text-sm font-medium text-gray-500">Email Address</dt>
                    <dd className="text-sm text-gray-900">{user.email}</dd>
                  </div>
                  <div>
                    <dt className="text-sm font-medium text-gray-500">Account Status</dt>
                    <dd className="text-sm text-gray-900">
                      <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                        user.isActive ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                      }`}>
                        {user.isActive ? 'Active' : 'Inactive'}
                      </span>
                    </dd>
                  </div>
                  <div>
                    <dt className="text-sm font-medium text-gray-500">Member Since</dt>
                    <dd className="text-sm text-gray-900">
                      {new Date(user.createdAt).toLocaleDateString()}
                    </dd>
                  </div>
                  {user.lastLoginAt && (
                    <div>
                      <dt className="text-sm font-medium text-gray-500">Last Login</dt>
                      <dd className="text-sm text-gray-900">
                        {new Date(user.lastLoginAt).toLocaleString()}
                      </dd>
                    </div>
                  )}
                </dl>
              </div>

              {/* Role Information */}
              <div>
                <h3 className="text-lg leading-6 font-medium text-gray-900 mb-4">
                  Role & Permissions
                </h3>
                <dl className="space-y-3">
                  <div>
                    <dt className="text-sm font-medium text-gray-500">Current Role</dt>
                    <dd className="text-sm text-gray-900">
                      <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                        user.role === UserRole.Admin ? 'bg-red-100 text-red-800' :
                        user.role === UserRole.Requester ? 'bg-blue-100 text-blue-800' :
                        user.role === UserRole.Approver ? 'bg-green-100 text-green-800' :
                        user.role === UserRole.Receiver ? 'bg-yellow-100 text-yellow-800' :
                        user.role === UserRole.Purchase ? 'bg-purple-100 text-purple-800' :
                        'bg-gray-100 text-gray-800'
                      }`}>
                        {getRoleName(user.role)}
                      </span>
                    </dd>
                  </div>
                  <div>
                    <dt className="text-sm font-medium text-gray-500">Role Description</dt>
                    <dd className="text-sm text-gray-900">{getRoleDescription(user.role)}</dd>
                  </div>
                </dl>
              </div>
            </div>
          </div>
        </div>

        {/* Account Actions */}
        <div className="bg-white shadow overflow-hidden sm:rounded-lg">
          <div className="px-4 py-5 sm:p-6">
            <h3 className="text-lg leading-6 font-medium text-gray-900 mb-4">
              Account Actions
            </h3>
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
              <button
                onClick={() => setShowChangePasswordModal(true)}
                className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                Change Password
              </button>
              <button
                className="bg-gray-500 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:ring-2 focus:ring-gray-500"
                disabled
              >
                Update Profile (Coming Soon)
              </button>
              <button
                className="bg-yellow-500 hover:bg-yellow-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:ring-2 focus:ring-yellow-500"
                disabled
              >
                Download Data (Coming Soon)
              </button>
            </div>
          </div>
        </div>

        {/* Security Information */}
        <div className="bg-white shadow overflow-hidden sm:rounded-lg">
          <div className="px-4 py-5 sm:p-6">
            <h3 className="text-lg leading-6 font-medium text-gray-900 mb-4">
              Security Information
            </h3>
            <div className="bg-blue-50 border-l-4 border-blue-400 p-4">
              <div className="flex">
                <div className="flex-shrink-0">
                  <svg className="h-5 w-5 text-blue-400" fill="currentColor" viewBox="0 0 20 20">
                    <path fillRule="evenodd" d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z" clipRule="evenodd" />
                  </svg>
                </div>
                <div className="ml-3">
                  <p className="text-sm text-blue-700">
                    <strong>Security Tips:</strong>
                  </p>
                  <ul className="text-sm text-blue-600 mt-1 list-disc list-inside">
                    <li>Use a strong, unique password</li>
                    <li>Change your password regularly</li>
                    <li>Never share your login credentials</li>
                    <li>Log out when using shared computers</li>
                  </ul>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <ChangePasswordModal
        isOpen={showChangePasswordModal}
        onClose={() => setShowChangePasswordModal(false)}
        onSuccess={handleChangePasswordSuccess}
      />
    </Layout>
  );
};

export default Profile;