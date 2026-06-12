"""agregar_campo_modelo

Revision ID: 48ab15582655
Revises: 114e9e3e7151
Create Date: 2026-06-12 18:58:46.006637

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '48ab15582655'
down_revision: Union[str, Sequence[str], None] = '114e9e3e7151'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    pass


def downgrade() -> None:
    """Downgrade schema."""
    pass
