import React, { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { Link } from 'react-router-dom';
import Layout from '../components/Layout';
import ChangePasswordModal from '../components/ChangePasswordModal';
import { UserRole } from '../types';

const Dashboard: React.FC = () => {
  const { user } = useAuth();
  const [showChangePasswordModal, setShowChangePasswordModal] = useState<boolean>(false);
  const [successMessage, setSuccessMessage] = useState<string>('');

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

  const getRoleActions = (role: number | undefined): string[] => {
    if (role === undefined) return ['Basic system access'];
    
    const actions: Record<number, string[]> = {
      [UserRole.Admin]: [
        'Manage user accounts',
        'View system reports',
        'Configure system settings',
        'Monitor user activities',
      ],
      [UserRole.Requester]: [
        'Create shopping requests',
        'Track request status',
        'Edit pending requests',
        'View request history',
      ],
      [UserRole.Approver]: [
        'Review pending requests',
        'Approve or reject requests',
        'Add approval comments',
        'View approval history',
      ],
      [UserRole.Receiver]: [
        'View incoming deliveries',
        'Confirm item receipt',
        'Report delivery issues',
        'Update inventory status',
      ],
      [UserRole.Purchase]: [
        'Process approved requests',
        'Contact suppliers',
        'Track purchase orders',
        'Update purchase status',
      ],
      [UserRole.User]: [
        'View assigned tasks',
        'Update profile information',
        'View notifications',
        'Access basic features',
      ],
    };
    
    return actions[role] || ['Basic system access'];
  };

  const isAdmin = (role: number | undefined): boolean => {
    return role === UserRole.Admin;
  };

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
      <div className="space-y-6">
        {successMessage && (
          <div className="bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded">
            {successMessage}
          </div>
        )}
        
        <div className="bg-white overflow-hidden shadow rounded-lg">
          <div className="px-4 py-5 sm:p-6">
            <h1 className="text-2xl font-bold text-gray-900 mb-4">
              Welcome, {user.firstName} {user.lastName}!
            </h1>
            <div className="bg-blue-50 border-l-4 border-blue-400 p-4">
              <div className="flex">
                <div className="ml-3">
                  <p className="text-sm text-blue-700">
                    You are logged in as <strong>{getRoleName(user.role)}</strong>
                  </p>
                  <p className="text-sm text-blue-600 mt-1">
                    {getRoleDescription(user.role)}
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div className="bg-white overflow-hidden shadow rounded-lg">
            <div className="px-4 py-5 sm:p-6">
              <h3 className="text-lg leading-6 font-medium text-gray-900 mb-4">
                Your Capabilities
              </h3>
              <ul className="space-y-2">
                {getRoleActions(user.role).map((action, index) => (
                  <li key={index} className="flex items-center">
                    <svg
                      className="h-4 w-4 text-green-500 mr-2"
                      fill="none"
                      stroke="currentColor"
                      viewBox="0 0 24 24"
                    >
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth={2}
                        d="M5 13l4 4L19 7"
                      />
                    </svg>
                    <span className="text-sm text-gray-700">{action}</span>
                  </li>
                ))}
              </ul>
            </div>
          </div>

          <div className="bg-white overflow-hidden shadow rounded-lg">
            <div className="px-4 py-5 sm:p-6">
              <h3 className="text-lg leading-6 font-medium text-gray-900 mb-4">
                Account Information
              </h3>
              <dl className="space-y-3">
                <div>
                  <dt className="text-sm font-medium text-gray-500">Email</dt>
                  <dd className="text-sm text-gray-900">{user.email}</dd>
                </div>
                <div>
                  <dt className="text-sm font-medium text-gray-500">Role</dt>
                  <dd className="text-sm text-gray-900">{getRoleName(user.role)} ({user.role})</dd>
                </div>
                <div>
                  <dt className="text-sm font-medium text-gray-500">Status</dt>
                  <dd className="text-sm text-gray-900">
                    <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800">
                      Active
                    </span>
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
          </div>
        </div>

        <div className="bg-white overflow-hidden shadow rounded-lg">
          <div className="px-4 py-5 sm:p-6">
            <h3 className="text-lg leading-6 font-medium text-gray-900 mb-4">
              Quick Actions
            </h3>
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
              <Link
                to="/profile"
                className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded text-center inline-block"
              >
                View Profile
              </Link>
              <button
                onClick={() => setShowChangePasswordModal(true)}
                className="bg-green-500 hover:bg-green-700 text-white font-bold py-2 px-4 rounded"
              >
                Change Password
              </button>
              {isAdmin(user.role) && (
                <Link
                  to="/admin"
                  className="bg-purple-500 hover:bg-purple-700 text-white font-bold py-2 px-4 rounded text-center inline-block"
                >
                  Admin Panel
                </Link>
              )}
              <Link
                to="/help"
                className="bg-gray-500 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded text-center inline-block"
              >
                Help & Support
              </Link>
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

export default Dashboard;