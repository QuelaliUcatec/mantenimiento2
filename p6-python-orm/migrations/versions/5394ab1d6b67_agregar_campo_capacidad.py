"""agregar_campo_capacidad

Revision ID: 5394ab1d6b67
Revises: 48ab15582655
Create Date: 2026-06-12 18:59:12.118325

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '5394ab1d6b67'
down_revision: Union[str, Sequence[str], None] = '48ab15582655'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    pass


def downgrade() -> None:
    """Downgrade schema."""
    pass
